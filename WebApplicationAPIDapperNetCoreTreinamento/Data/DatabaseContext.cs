using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplicationAPIDapperNetCoreTreinamento.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string not found");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
