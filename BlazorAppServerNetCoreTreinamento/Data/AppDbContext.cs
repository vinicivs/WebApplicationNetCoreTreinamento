using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace BlazorAppServerNetCoreTreinamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cep> Cep
        {
            get; set;
        }
    }

    [Table("Cep")] // nome da tabela no banco
    public class Cep
    {
        public int Id { get; set; }

        [Column("Cep")]
        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Formato válido: 00000-000")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório")]
        [StringLength(200, ErrorMessage = "Máximo de 200 caracteres")] 
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório")]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória")]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, ErrorMessage = "Use a sigla do estado (ex: SP)")]
        public string Uf { get; set; }
    }
}
