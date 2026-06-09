using WebApplicationAPINetCoreTreinamento.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAPINetCoreTreinamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ceps> Ceps
        {
            get; set;
        }
    }
}
