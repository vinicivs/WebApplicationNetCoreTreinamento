using Microsoft.EntityFrameworkCore;
using WebApplicationAPIGraphQLNetCoreTreinamento.Models;
using WebApplicationAPIGraphQLNetCoreTreinamento.Data;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.Repositories
{
    public class CepRepository : ICepRepository
    {
        private readonly ApplicationDbContext _context;

        public CepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cep>> GetAllAsync()
        {
            return await _context.Ceps.ToListAsync();
        }

        public async Task<Cep?> GetByIdAsync(int id)
        {
            return await _context.Ceps.FindAsync(id);
        }

        public async Task<Cep?> GetByCodigoAsync(string codigo)
        {
            return await _context.Ceps.FirstOrDefaultAsync(c => c.Codigo == codigo);
        }

        public async Task<Cep> CreateAsync(Cep cep)
        {
            //cep.DataCriacao = DateTime.Now;
            _context.Ceps.Add(cep);
            await _context.SaveChangesAsync();
            return cep;
        }

        public async Task<Cep> UpdateAsync(Cep cep)
        {
            //cep.DataAtualizacao = DateTime.Now;
            _context.Entry(cep).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cep;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cep = await _context.Ceps.FindAsync(id);
            if (cep == null)
                return false;

            _context.Ceps.Remove(cep);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Ceps.AnyAsync(c => c.Id == id);
        }
    }
}
