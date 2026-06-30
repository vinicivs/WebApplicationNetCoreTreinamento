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
                ?? throw new ArgumentNullException("Connection string não encontrada"); //("Connection string not found");
        }

        public virtual IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        // Para criar uma conexão com o banco de dados, você pode usar o método CreateConnection. Este método retorna uma nova instância de SqlConnection usando a string de conexão fornecida no construtor da classe DatabaseContext.
    }
}
