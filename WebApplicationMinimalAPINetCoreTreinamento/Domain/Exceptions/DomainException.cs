namespace WebApplicationMinimalAPINetCoreTreinamento.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
        protected DomainException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, string identifier)
            : base($"Entidade '{entityName}' com identificador '{identifier}' não encontrada.") { }
    }

    public class EntityAlreadyExistsException : DomainException
    {
        public EntityAlreadyExistsException(string entityName, string identifier)
            : base($"Entidade '{entityName}' com identificador '{identifier}' já existe.") { }
    }

    public class InvalidCepException : DomainException
    {
        public InvalidCepException(string cep)
            : base($"CEP '{cep}' é inválido. Deve conter 8 dígitos.") { }
    }

    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
