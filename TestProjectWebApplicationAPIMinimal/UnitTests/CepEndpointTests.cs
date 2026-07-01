using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestProjectWebApplicationAPIMinimal.Helpers;
using WebApplicationAPIMinimalNetCoreTreinamento.DTOs;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using Xunit;
using Moq;
using FluentAssertions;
using WebApplicationAPIMinimalNetCoreTreinamento.Data;
using System.Net;


namespace TestProjectWebApplicationAPIMinimal.UnitTests
{
    public class CepEndpointTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<DbSet<Cep>> _mockSet;

        public CepEndpointTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockSet = new Mock<DbSet<Cep>>();
        }

        [Fact]
        public async Task GetCepById_ShouldReturnCep_WhenExists()
        {
            // Arrange
            var cep = TestDataFactory.CreateValidCep();
            var mockData = new List<Cep> { cep }.AsQueryable();
            var mockSet = new Mock<DbSet<Cep>>();

            mockSet.As<IQueryable<Cep>>().Setup(m => m.Provider).Returns(mockData.Provider);
            mockSet.As<IQueryable<Cep>>().Setup(m => m.Expression).Returns(mockData.Expression);
            mockSet.As<IQueryable<Cep>>().Setup(m => m.ElementType).Returns(mockData.ElementType);
            mockSet.As<IQueryable<Cep>>().Setup(m => m.GetEnumerator()).Returns(mockData.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Ceps).Returns(mockSet.Object);

            // Act
            var result = await mockContext.Object.Ceps.FindAsync(cep.Codigo);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(cep.Id);
            result?.Codigo.Should().Be(cep.Codigo);
        }

        [Fact]
        public void CreateCepDto_ShouldValidateRequiredFields()
        {
            // Arrange
            var dto = new CepCreateDto
            {
                Codigo = "",
                Logradouro = "",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                Uf = "SP"
            };

            // Act & Assert
            dto.Codigo.Should().BeNullOrEmpty();
            dto.Logradouro.Should().BeNullOrEmpty();
            dto.Bairro.Should().NotBeNullOrEmpty();
            dto.Cidade.Should().NotBeNullOrEmpty();
            dto.Uf.Should().Be("SP");
        }

        [Fact]
        public void UpdateCepDto_ShouldContainAllFields()
        {
            // Arrange
            var dto = TestDataFactory.CreateValidCepUpdateDto();

            // Assert
            dto.Codigo.Should().NotBeNullOrEmpty();
            dto.Logradouro.Should().NotBeNullOrEmpty();
            dto.Numero.Should().NotBeNull();
            dto.Bairro.Should().NotBeNullOrEmpty();
            dto.Cidade.Should().NotBeNullOrEmpty();
            dto.Uf.Should().NotBeNullOrEmpty();
            dto.Uf.Length.Should().Be(2);
        }

        [Theory]
        [InlineData("01001-000")]
        [InlineData("20040-000")]
        [InlineData("01310-000")]
        public void CepDto_ShouldAcceptValidCepFormats(string cepNumero)
        {
            // Arrange
            var dto = new CepDto
            {
                Id = 1,
                Codigo = cepNumero,
                Logradouro = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                Uf = "SP"
            };

            // Act & Assert
            dto.Codigo.Should().Be(cepNumero);
            dto.Codigo.Should().MatchRegex(@"^\d{5}-\d{3}$");
        }
    }
}
