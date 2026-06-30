using WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities;

namespace WebApplicationMinimalAPINetCoreTreinamento.Domain.Interfaces
{
    public interface ICepRepository : IRepository<Cep>
    {
        Task<Cep?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cep>> GetByCidadeAsync(string cidade, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cep>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cep>> GetActiveCepsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Cep>> GetCepsByBairroAsync(string bairro, CancellationToken cancellationToken = default);
    }
}
