using WebApplicationMinimalAPINetCoreTreinamento.Domain.Common;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Events;
namespace WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities
{
    public class Cep : BaseEntity
    {
        public string Codigo { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Uf { get; private set; }
        public DateTime UltimaAtualizacao { get; private set; }
        public bool Ativo { get; private set; }
        public int Versao { get; private set; }

        private Cep() { } // EF Core

        public Cep(string codigo, string logradouro, string numero, string complemento, string bairro, string cidade, string uf)
        {
            Codigo = NormalizeCep(codigo);
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
        }

        public void Atualizar(string logradouro, string numero, string complemento, string bairro, string cidade, string uf)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
        }

        public void Desativar()
        {
            Ativo = false;
            UltimaAtualizacao = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Ativar()
        {
            Ativo = true;
            UltimaAtualizacao = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        private static string NormalizeCep(string cep)
        {
            return new string(cep.Where(char.IsDigit).ToArray());
        }
    }
}
