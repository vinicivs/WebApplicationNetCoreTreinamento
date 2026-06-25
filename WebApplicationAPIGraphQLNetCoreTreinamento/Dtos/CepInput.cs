using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.Dtos
{
    public class CepInput
    {
        [Required]
        [StringLength(9)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Logradouro { get; set; } = string.Empty;

        [StringLength(50)]
        public string Numero { get; set; } = string.Empty;

        [StringLength(100)]
        public string Complemento { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string Uf { get; set; } = string.Empty;
        
    }

    public class CepUpdateInput : CepInput
    {
        public int Id { get; set; }
    }
}
