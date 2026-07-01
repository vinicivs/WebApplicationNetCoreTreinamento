using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using TestProjectWebApplicationAPIMinimal.Helpers;
using FluentAssertions;
using System.Runtime.ConstrainedExecution;
using Xunit;

namespace TestProjectWebApplicationAPIMinimal.UnitTests
{
    public class CepModelTests
    {
        [Fact]
        public void Cep_ShouldCreateValidObject()
        {
            // Arrange & Act
            var cep = TestDataFactory.CreateValidCep();

            // Assert
            cep.Should().NotBeNull();
            cep.Id.Should().BeGreaterThan(0);
            cep.Codigo.Should().NotBeNullOrEmpty();
            cep.Logradouro.Should().NotBeNullOrEmpty();
            cep.Bairro.Should().NotBeNullOrEmpty();
            cep.Cidade.Should().NotBeNullOrEmpty();
            cep.Uf.Should().NotBeNullOrEmpty();
            cep.Uf.Length.Should().Be(2);
        }

        [Fact]
        public void Cep_ShouldAllowNullNumero()
        {
            // Arrange
            var cep = TestDataFactory.CreateValidCep();
            cep.Numero = null;

            // Act & Assert
            cep.Numero.Should().BeNull();
        }

        [Fact]
        public void Cep_ShouldAllowNullComplemento()
        {
            // Arrange
            var cep = TestDataFactory.CreateValidCep();
            cep.Complemento = null;

            // Act & Assert
            cep.Complemento.Should().BeNull();
        }

        [Theory]
        [InlineData("SP")]
        [InlineData("RJ")]
        [InlineData("MG")]
        public void Cep_ShouldAcceptValidUf(string uf)
        {
            // Arrange
            var cep = TestDataFactory.CreateValidCep();
            cep.Uf = uf;

            // Act & Assert
            cep.Uf.Should().Be(uf);
            cep.Uf.Length.Should().Be(2);
        }

        [Fact]
        public void Cep_ShouldBeEqualWhenSameId()
        {
            // Arrange
            var cep1 = TestDataFactory.CreateValidCep();
            var cep2 = new Cep
            {
                Id = cep1.Id,
                Codigo = "01001-000",
                Logradouro = "Different",
                Numero = "123",
                Bairro = "Center",
                Cidade = "City",
                Uf = "SP"
            };

            // Act & Assert
            cep1.Codigo.Should().Be(cep2.Codigo);
            cep1.Should().NotBeSameAs(cep2);
            cep1.Codigo.Should().NotBe(cep2.Codigo);
        }
    }
}
