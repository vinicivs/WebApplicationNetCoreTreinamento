using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka;

namespace WebApplicationMinimalAPINetCoreTreinamento.API.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";
                await command.ExecuteScalarAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is not available", ex);
            }
        }
    }

    public class KafkaHealthCheck : IHealthCheck
    {
        private readonly IKafkaProducerService _producer;
        private readonly KafkaConfig _config;

        public KafkaHealthCheck(IKafkaProducerService producer, IOptions<KafkaConfig> options)
        {
            _producer = producer;
            _config = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _producer.ProduceAsync(_config.Topic, "health-check", new { Health = "check" }, cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Kafka is not available", ex);
            }
        }
    }
}
