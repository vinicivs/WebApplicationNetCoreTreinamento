using WebApplicationAPIDapperNetCoreTreinamento.Data;
using WebApplicationAPIDapperNetCoreTreinamento.Models;
using Dapper;
using System.Data;

namespace WebApplicationAPIDapperNetCoreTreinamento.Repositories
{
    public interface ICepRepository
    {
        Task<IEnumerable<Cep>> GetAllAsync();
        Task<Cep?> GetByIdAsync(int id);
        Task<Cep?> GetByCepAsync(string cep);
        Task<int> CreateAsync(Cep cep);
        Task<bool> UpdateAsync(Cep cep);
        Task<bool> DeleteAsync(int id);
    }

    public class CepRepository : ICepRepository
    {
        private readonly DatabaseContext _context;

        public CepRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cep>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Cep ORDER BY Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Cep>(sql);
        }

        public async Task<Cep?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Cep WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Cep>(sql, new { Id = id });
        }

        public async Task<Cep?> GetByCepAsync(string cep)
        {
            const string sql = "SELECT * FROM Cep WHERE Cep = @Cep";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Cep>(sql, new { Cep = cep });
        }

        public async Task<int> CreateAsync(Cep cep)
        {
            const string sql = @"
            INSERT INTO Cep (Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Uf)
            VALUES (@Codigo, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @Uf);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(sql, cep);
        }

        public async Task<bool> UpdateAsync(Cep cep)
        {
            const string sql = @"
            UPDATE Cep 
            SET Cep = @Codigo, 
                Logradouro = @Logradouro, 
                Numero = @Numero,
                Complemento = @Complemento,
                Bairro = @Bairro, 
                Cidade = @Cidade, 
                Uf = @Uf
            WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, cep);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Cep WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
