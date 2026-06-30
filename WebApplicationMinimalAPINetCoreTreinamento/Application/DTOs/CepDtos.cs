using System.Text.Json.Serialization;

namespace WebApplicationMinimalAPINetCoreTreinamento.Application.DTOs
{
    public class CepDtos
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        //public string Pais { get; set; }
        //public string? Complemento { get; set; }
        //public string? UnidadeFederativa { get; set; }
        //public string? Ibge { get; set; }
        //public string? Gia { get; set; }
        //public string? Ddd { get; set; }
        //public string? Siafi { get; set; }
        //public DateTime UltimaAtualizacao { get; set; }
        //public bool Ativo { get; set; }
        //public int Versao { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCepDto
    {
        public string Codigo { get; set; }
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        //public string Pais { get; set; }
        //public string? Complemento { get; set; }
        //public string? UnidadeFederativa { get; set; }
        //public string? Ibge { get; set; }
        //public string? Gia { get; set; }
        //public string? Ddd { get; set; }
        //public string? Siafi { get; set; }
    }

    public class UpdateCepDto
    {
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        //public string Pais { get; set; }
        //public string? Complemento { get; set; }
        //public string? UnidadeFederativa { get; set; }
        //public string? Ibge { get; set; }
        //public string? Gia { get; set; }
        //public string? Ddd { get; set; }
        //public string? Siafi { get; set; }
    }

    public class CepFilterDto
    {
        public string? Codigo { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Bairro { get; set; }
        //public bool? Ativo { get; set; }
        //public int PageNumber { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
    }

    public class PagedResultDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
