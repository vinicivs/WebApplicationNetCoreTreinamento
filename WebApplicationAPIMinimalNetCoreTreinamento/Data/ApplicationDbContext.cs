using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;

namespace WebApplicationAPIMinimalNetCoreTreinamento.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Cep> Ceps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Numero).HasMaxLength(10);
                entity.Property(e => e.Complemento).HasMaxLength(100);
                entity.Property(e => e.Bairro).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Cidade).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Uf).IsRequired().HasMaxLength(2);

                entity.HasIndex(e => e.Codigo).IsUnique();
            });
        }
    }

}
