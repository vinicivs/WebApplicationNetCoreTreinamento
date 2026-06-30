using Confluent.Kafka;
using MediatR;
using System.Text.Json;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Events;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka;
using System.Text.Json;

namespace WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.EventHandlers
{
    public class KafkaEventHandler :
    INotificationHandler<CepCreatedEvent>,
    INotificationHandler<CepUpdatedEvent>,
    INotificationHandler<CepDeletedEvent>
    {
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<KafkaEventHandler> _logger;

        public KafkaEventHandler(IKafkaProducerService kafkaProducer, ILogger<KafkaEventHandler> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task Handle(CepCreatedEvent notification, CancellationToken cancellationToken)
        {
            await PublishEvent(notification, "cep.created", cancellationToken);
        }

        public async Task Handle(CepUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await PublishEvent(notification, "cep.updated", cancellationToken);
        }

        public async Task Handle(CepDeletedEvent notification, CancellationToken cancellationToken)
        {
            await PublishEvent(notification, "cep.deleted", cancellationToken);
        }

        private async Task PublishEvent<T>(T @event, string eventType, CancellationToken cancellationToken) where T : CepEvent
        {
            try
            {
                var message = new Message<string, string>
                {
                    Key = @event.CepCodigo,
                    Value = JsonSerializer.Serialize(@event),
                    Headers = new Headers
                {
                    { "event-type", System.Text.Encoding.UTF8.GetBytes(eventType) },
                    { "timestamp", System.Text.Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString("o")) }
                }
                };

                await _kafkaProducer.PublishAsync(message, cancellationToken);
                _logger.LogInformation($"Event {eventType} published for CEP {@event.CepCodigo}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing event {eventType} for CEP {@event.CepCodigo}");
                throw;
            }
        }
    }
}
