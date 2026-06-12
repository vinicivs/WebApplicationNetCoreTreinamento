using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationMVCNetCoreTreinamento.Models
{
    [Table("Cep")]
    public class Cep
    {
        public int Id { get; set; }

        [Column("Cep")]                 // nome da coluna no banco
        [Display(Name = "Cep")]        
        public string Codigo { get; set; } = string.Empty;
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}
