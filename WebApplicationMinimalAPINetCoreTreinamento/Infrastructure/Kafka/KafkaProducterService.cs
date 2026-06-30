using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
        public string DlqTopic { get; set; }
        public int RetryCount { get; set; } = 3;
        public int RetryBackoffMs { get; set; } = 1000;
        public bool EnableIdempotence { get; set; } = true;
        public int MaxInFlight { get; set; } = 5;
        public int MaxRetryAttempts { get; set; } = 3;
        public int RetryBaseDelayMs { get; set; } = 100;
        public int RetryMaxDelayMs { get; set; } = 1000;
        public bool EnableSsl { get; set; } = false;
        public string? SslCaLocation { get; set; }
        public string? SslCertificateLocation { get; set; }
        public string? SslKeyLocation { get; set; }
    }

    public interface IKafkaProducerService : IDisposable
    {
        Task ProduceAsync<T>(string topic, string key, T value, CancellationToken cancellationToken = default);
        Task ProduceWithRetryAsync<T>(string topic, string key, T value, CancellationToken cancellationToken = default);
    }

    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfig _config;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IOptions<KafkaConfig> options, ILogger<KafkaProducerService> logger)
        {
            _config = options.Value;
            _logger = logger;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = _config.EnableIdempotence,
                MaxInFlight = _config.MaxInFlight,
                MessageSendMaxRetries = _config.MaxRetryAttempts,
                RetryBackoffMs = _config.RetryBaseDelayMs,
                MetadataMaxAgeMs = 60000,
                EnableSslCertificateVerification = _config.EnableSsl,
                SslCaLocation = _config.SslCaLocation,
                SslCertificateLocation = _config.SslCertificateLocation,
                SslKeyLocation = _config.SslKeyLocation
            };

            _producer = new ProducerBuilder<string, string>(producerConfig)
                .SetErrorHandler((_, error) => _logger.LogError($"Kafka Producer Error: {error.Reason}"))
                .SetStatisticsHandler((_, stats) => _logger.LogDebug($"Kafka Statistics: {stats}"))
                .Build();
        }

        public async Task ProduceAsync<T>(string topic, string key, T value, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new Message<string, string>
                {
                    Key = key,
                    Value = JsonSerializer.Serialize(value, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }),
                    Headers = new Headers
                {
                    { "message-type", System.Text.Encoding.UTF8.GetBytes(typeof(T).Name) },
                    { "timestamp", System.Text.Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("o")) },
                    { "source", System.Text.Encoding.UTF8.GetBytes("cep-api") }
                }
                };

                var deliveryResult = await _producer.ProduceAsync(topic, message, cancellationToken);

                _logger.LogInformation(
                    "Message delivered to topic {Topic}, partition {Partition}, offset {Offset}, key: {Key}",
                    topic, deliveryResult.Partition, deliveryResult.Offset, key);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Failed to produce message to topic {Topic}", topic);

                // Send to DLQ if configured
                if (!string.IsNullOrEmpty(_config.DlqTopic))
                {
                    await SendToDeadLetterQueue(topic, key, value, ex, cancellationToken);
                }

                throw new KafkaProducerException($"Failed to produce message to topic {topic}", ex);
            }
        }

        public async Task ProduceWithRetryAsync<T>(string topic, string key, T value, CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            int delay = _config.RetryBaseDelayMs;

            while (attempt < _config.RetryCount)
            {
                try
                {
                    await ProduceAsync(topic, key, value, cancellationToken);
                    return;
                }
                catch (Exception ex) when (attempt < _config.RetryCount - 1)
                {
                    attempt++;
                    _logger.LogWarning(ex, "Retry {Attempt} for message key {Key}", attempt, key);
                    await Task.Delay(delay, cancellationToken);
                    delay = Math.Min(delay * 2, _config.RetryMaxDelayMs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "All retry attempts failed for message key {Key}", key);
                    throw;
                }
            }
        }

        private async Task SendToDeadLetterQueue<T>(string originalTopic, string key, T value, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                var dlqMessage = new
                {
                    OriginalTopic = originalTopic,
                    Key = key,
                    Value = value,
                    Error = exception.Message,
                    ErrorType = exception.GetType().Name,
                    StackTrace = exception.StackTrace,
                    Timestamp = DateTime.UtcNow,
                    RetryCount = 0
                };

                await ProduceAsync(_config.DlqTopic, key, dlqMessage, cancellationToken);
                _logger.LogWarning("Message sent to DLQ topic {DlqTopic}", _config.DlqTopic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to DLQ for original topic {OriginalTopic}", originalTopic);
            }
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
    }

    public class KafkaProducerException : Exception
    {
        public KafkaProducerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
