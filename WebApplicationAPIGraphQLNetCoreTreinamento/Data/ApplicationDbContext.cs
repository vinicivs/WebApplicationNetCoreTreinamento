using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPIGraphQLNetCoreTreinamento.Models;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Cep> Ceps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed de dados iniciais
            modelBuilder.Entity<Cep>().HasData(
                new Cep
                {
                    Id = 100,
                    Codigo = "01001-010",
                    Logradouro = "Praça da Sé",
                    Numero = "123",
                    Complemento = "Compl",
                    Bairro = "Sé",
                    Cidade = "São Paulo",
                    Uf = "SP"
                },
                new Cep
                {
                    Id = 200,
                    Codigo = "20040-020",
                    Logradouro = "Avenida Presidente Vargas",
                    Numero = "321",
                    Complemento = "Complemento rio",
                    Bairro = "Centro",
                    Cidade = "Rio de Janeiro",
                    Uf = "RJ"
                }
            );
        }
    }

}
