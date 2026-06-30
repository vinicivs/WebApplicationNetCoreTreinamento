using WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Interfaces;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Repositories
{
    public class CepRepository : ICepRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Cep> _dbSet;

        public CepRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Cep>();
        }

        public async Task<Cep?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Cep?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Codigo == codigo, cancellationToken);
        }

        public async Task<IEnumerable<Cep>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .OrderBy(c => c.Codigo)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cep>> GetByCidadeAsync(string cidade, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.Cidade.Contains(cidade))
                .OrderBy(c => c.Codigo)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cep>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.Uf == estado.ToUpper())
                .OrderBy(c => c.Cidade)
                .ThenBy(c => c.Codigo)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(c => c.Codigo == codigo, cancellationToken);
        }

        public async Task<IEnumerable<Cep>> GetActiveCepsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.Ativo)
                .OrderBy(c => c.Codigo)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cep>> GetCepsByBairroAsync(string bairro, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.Bairro.Contains(bairro))
                .OrderBy(c => c.Cidade)
                .ThenBy(c => c.Codigo)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Cep entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(Cep entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Cep entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
