using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApplicationMinimalAPINetCoreTreinamento.Application.Services;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Events;

namespace WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka
{
    public interface IKafkaMessageProcessor
    {
        Task ProcessMessageAsync(string key, string value, CancellationToken cancellationToken);
    }

    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IKafkaMessageProcessor _messageProcessor;
        private readonly KafkaConfig _config;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumerService(
            IOptions<KafkaConfig> options,
            IKafkaMessageProcessor messageProcessor,
            ILogger<KafkaConsumerService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _config = options.Value;
            _messageProcessor = messageProcessor;
            _logger = logger;
            _scopeFactory = scopeFactory;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = $"{_config.GroupId}-{Guid.NewGuid():N}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                MaxPollIntervalMs = 300000,
                SessionTimeoutMs = 6000,
                HeartbeatIntervalMs = 3000,
                EnablePartitionEof = true,
                StatisticsIntervalMs = 5000,
                FetchMaxBytes = 1024 * 1024,
                FetchWaitMaxMs = 500,
                MaxPartitionFetchBytes = 1024 * 1024,
                EnableSslCertificateVerification = _config.EnableSsl,
                SslCaLocation = _config.SslCaLocation,
                SslCertificateLocation = _config.SslCertificateLocation,
                SslKeyLocation = _config.SslKeyLocation
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetErrorHandler((_, error) => _logger.LogError($"Kafka Consumer Error: {error.Reason}"))
                .SetPartitionsAssignedHandler((_, partitions) =>
                    _logger.LogInformation($"Assigned partitions: {string.Join(", ", partitions)}"))
                .SetPartitionsRevokedHandler((_, partitions) =>
                    _logger.LogInformation($"Revoked partitions: {string.Join(", ", partitions)}"))
                .SetPartitionsLostHandler((_, partitions) =>
                    _logger.LogWarning($"Lost partitions: {string.Join(", ", partitions)}"))
                .Build();

            _consumer.Subscribe(_config.Topic);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await ProcessMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer encountered an error");
                throw;
            }
        }

        private async Task ProcessMessagesAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult == null || consumeResult.Message == null)
                    {
                        await Task.Delay(100, stoppingToken);
                        continue;
                    }

                    await ProcessMessageWithRetryAsync(consumeResult, stoppingToken);

                    _consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Error}", ex.Error.Reason);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            _consumer.Close();
        }

        private async Task ProcessMessageWithRetryAsync(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
        {
            const int maxRetries = 3;
            int retryCount = 0;
            int delay = 1000;

            while (retryCount < maxRetries)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var messageProcessor = scope.ServiceProvider.GetRequiredService<IKafkaMessageProcessor>();

                    await messageProcessor.ProcessMessageAsync(
                        consumeResult.Message.Key,
                        consumeResult.Message.Value,
                        cancellationToken);

                    _logger.LogInformation(
                        "Message processed successfully. Topic: {Topic}, Partition: {Partition}, Offset: {Offset}",
                        consumeResult.Topic, consumeResult.Partition, consumeResult.Offset);

                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex,
                        "Failed to process message (Attempt {Retry}/{MaxRetries}). Key: {Key}",
                        retryCount, maxRetries, consumeResult.Message.Key);

                    if (retryCount >= maxRetries)
                    {
                        _logger.LogError(ex,
                            "Max retries reached for message. Key: {Key}. Sending to DLQ",
                            consumeResult.Message.Key);

                        await SendToDeadLetterQueue(consumeResult, ex, cancellationToken);
                    }
                    else
                    {
                        await Task.Delay(delay * retryCount, cancellationToken);
                    }
                }
            }
        }

        private async Task SendToDeadLetterQueue(ConsumeResult<string, string> consumeResult, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var producer = scope.ServiceProvider.GetRequiredService<IKafkaProducerService>();

                var dlqMessage = new
                {
                    OriginalTopic = consumeResult.Topic,
                    OriginalPartition = consumeResult.Partition,
                    OriginalOffset = consumeResult.Offset,
                    Key = consumeResult.Message.Key,
                    Value = consumeResult.Message.Value,
                    Error = exception.Message,
                    ErrorType = exception.GetType().Name,
                    StackTrace = exception.StackTrace,
                    Headers = consumeResult.Message.Headers.ToDictionary(h => h.Key, h =>
                        System.Text.Encoding.UTF8.GetString(h.GetValueBytes())),
                    Timestamp = DateTime.UtcNow
                };

                await producer.ProduceAsync(
                    _config.DlqTopic,
                    consumeResult.Message.Key,
                    dlqMessage,
                    cancellationToken);

                _logger.LogWarning("Message sent to DLQ. Topic: {Topic}, Key: {Key}",
                    _config.DlqTopic, consumeResult.Message.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to DLQ. Topic: {Topic}, Key: {Key}",
                    _config.DlqTopic, consumeResult.Message.Key);
            }
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }

    // Implementation of message processor
    public class CepMessageProcessor : IKafkaMessageProcessor
    {
        private readonly ICepService _cepService;
        private readonly ILogger<CepMessageProcessor> _logger;

        public CepMessageProcessor(ICepService cepService, ILogger<CepMessageProcessor> logger)
        {
            _cepService = cepService;
            _logger = logger;
        }

        public async Task ProcessMessageAsync(string key, string value, CancellationToken cancellationToken)
        {
            try
            {
                // Parse message based on event type
                var message = JsonSerializer.Deserialize<CepEvent>(value, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (message == null)
                {
                    _logger.LogWarning("Invalid message format. Key: {Key}", key);
                    return;
                }

                _logger.LogInformation("Processing event: {EventType}, CEP: {CepCodigo}",
                    message.EventType, message.CepCodigo);

                // Process based on event type
                switch (message.EventType)
                {
                    case "CEP_CREATED":
                        // Process creation event
                        break;
                    case "CEP_UPDATED":
                        // Process update event
                        break;
                    case "CEP_DELETED":
                        // Process deletion event
                        break;
                    default:
                        _logger.LogWarning("Unknown event type: {EventType}", message.EventType);
                        break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize message. Key: {Key}", key);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message. Key: {Key}", key);
                throw;
            }
        }
    }
}
