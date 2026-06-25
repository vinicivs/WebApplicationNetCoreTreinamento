using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPIDapperNetCoreTreinamento.DTOs
{
    public class CepDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string? Numero { get; set; } = string.Empty;

        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
    }

    public class CepResponseDto : CepDto
    {
        public int Id { get; set; }
        //public DateTime DataCriacao { get; set; }
        //public DateTime? DataAtualizacao { get; set; }
    }
}
