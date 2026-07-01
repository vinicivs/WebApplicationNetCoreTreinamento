using Microsoft.EntityFrameworkCore;

namespace WebApplicationAPIMySqlNetCoreTreinamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cep> Ceps { get; set; }
    }

    public class Cep
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}
