using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPIDapperNetCoreTreinamento.Models
{
    [Table("Cep")] // nome da tabela no banco
    public class Cep
    {
        public int Id { get; set; }
        [Column("Cep")]
        public string Codigo { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string? Numero { get; set; } = string.Empty;

        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        
    }
}
