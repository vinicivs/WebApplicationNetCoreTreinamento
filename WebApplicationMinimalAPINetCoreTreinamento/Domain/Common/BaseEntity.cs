namespace WebApplicationMinimalAPINetCoreTreinamento.Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public string CreatedBy { get; protected set; }
        public string? UpdatedBy { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public void SetCreatedBy(string user) => CreatedBy = user;
        public void SetUpdatedBy(string user) => UpdatedBy = user;
    }
}
