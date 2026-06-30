using MediatR;

namespace WebApplicationMinimalAPINetCoreTreinamento.Domain.Events
{
    public abstract class CepEvent : INotification
    {
        public string EventId { get; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public string CepCodigo { get; set; }
        public string EventType { get; protected set; }
        public int Version { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class CepCreatedEvent : CepEvent
    {
        public CepCreatedEvent(string cepCodigo)
        {
            CepCodigo = cepCodigo;
            EventType = "CEP_CREATED";
            Version = 1;
        }
    }

    public class CepUpdatedEvent : CepEvent
    {
        public CepUpdatedEvent(string cepCodigo)
        {
            CepCodigo = cepCodigo;
            EventType = "CEP_UPDATED";
            Version = 1;
        }
    }

    public class CepDeletedEvent : CepEvent
    {
        public CepDeletedEvent(string cepCodigo)
        {
            CepCodigo = cepCodigo;
            EventType = "CEP_DELETED";
            Version = 1;
        }
    }

    public class CepReactivatedEvent : CepEvent
    {
        public CepReactivatedEvent(string cepCodigo)
        {
            CepCodigo = cepCodigo;
            EventType = "CEP_REACTIVATED";
            Version = 1;
        }
    }
}
