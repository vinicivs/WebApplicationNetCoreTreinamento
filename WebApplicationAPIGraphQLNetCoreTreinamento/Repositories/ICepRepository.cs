using WebApplicationAPIGraphQLNetCoreTreinamento.Models;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.Repositories
{
    public interface ICepRepository
    {
        Task<IEnumerable<Cep>> GetAllAsync();
        Task<Cep?> GetByIdAsync(int id);
        Task<Cep?> GetByCodigoAsync(string codigo);
        Task<Cep> CreateAsync(Cep cep);
        Task<Cep> UpdateAsync(Cep cep);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
