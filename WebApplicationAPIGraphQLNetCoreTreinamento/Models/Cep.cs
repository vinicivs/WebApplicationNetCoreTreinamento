using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.Models
{
    [Table("Cep")] // nome da tabela no banco
    public class Cep
    {
        [Key]
        public int Id { get; set; }

        [Column("Cep")]
        [Required]
        [StringLength(9)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string Logradouro { get; set; }

        [StringLength(50)]
        public string? Numero { get; set; }

        [StringLength(100)]
        public string? Complemento { get; set; }

        [Required]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2)]
        public string Uf { get; set; }
    }
}
