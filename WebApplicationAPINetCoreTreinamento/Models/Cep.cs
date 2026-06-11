using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;// swagger nomeado
using System.Text.Json.Serialization;// json nomeado

namespace WebApplicationAPINetCoreTreinamento.Models
{
    [Table("Cep")] // nome da tabela no banco
    public class Cep
    {
        public int Id { get; set; }

        [Column("Cep")]                 // nome da coluna no banco
        [Display(Name = "Cep")]         // nome amigável exibido no Swagger
        [SwaggerSchema("Cep do endereço")] // descrição no Swagger
        [JsonPropertyName("cep")] // nome no JSON retornado
        public string Codigo { get; set; } = string.Empty;
        public string Logradouro { get; set; }
        public string? Numero { get; set; } 
        public string? Complemento { get; set; } 
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}
