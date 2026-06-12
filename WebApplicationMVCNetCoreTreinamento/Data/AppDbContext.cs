using WebApplicationMVCNetCoreTreinamento.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationMVCNetCoreTreinamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cep> Cep
        {
            get; set;
        }

    }
}
