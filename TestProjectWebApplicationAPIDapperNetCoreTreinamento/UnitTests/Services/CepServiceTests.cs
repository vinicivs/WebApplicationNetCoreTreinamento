using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationAPIDapperNetCoreTreinamento.DTOs;
using WebApplicationAPIDapperNetCoreTreinamento.Models;
using WebApplicationAPIDapperNetCoreTreinamento.Repositories;
using WebApplicationAPIDapperNetCoreTreinamento.Services;

namespace TestProjectWebApplicationAPIDapperNetCoreTreinamento.UnitTests.Services
{
    public class CepServiceTests
    {
        private readonly Mock<ICepRepository> _mockRepository;
        private readonly ICepService _service;

        public CepServiceTests()
        {
            _mockRepository = new Mock<ICepRepository>();
            _service = new CepService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCeps()
        {
            // Arrange
            var ceps = new List<Cep>
        {
            new() { Id = 1, Codigo = "01001000", Logradouro = "Praça da Sé" },
            new() { Id = 2, Codigo = "20040002", Logradouro = "Av. Presidente Vargas" }
        };

            _mockRepository.Setup(r => r.GetAllAsync())
                          .ReturnsAsync(ceps);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<CepResponseDto>();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCep()
        {
            // Arrange
            var cep = new Cep
            {
                Id = 1,
                Codigo = "01001000",
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                          .ReturnsAsync(cep);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Codigo.Should().Be("01001000");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                          .ReturnsAsync((Cep?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateCep()
        {
            // Arrange
            var cepDto = new CepDto
            {
                Codigo = "12345678",
                Logradouro = "Rua Teste",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            _mockRepository.Setup(r => r.GetByCepAsync(cepDto.Codigo))
                          .ReturnsAsync((Cep?)null);

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Cep>()))
                          .ReturnsAsync(3);

            // Act
            var result = await _service.CreateAsync(cepDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(3);
            result.Codigo.Should().Be("12345678");
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Cep>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateCep_ShouldThrowException()
        {
            // Arrange
            var cepDto = new CepDto
            {
                Codigo = "01001000",
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            var existingCep = new Cep { Id = 1, Codigo = "01001000" };
            _mockRepository.Setup(r => r.GetByCepAsync(cepDto.Codigo))
                          .ReturnsAsync(existingCep);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(cepDto)
            );
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateCep()
        {
            // Arrange
            var cepDto = new CepDto
            {
                Codigo = "01001000",
                Logradouro = "Praça da Sé Atualizada",
                Bairro = "Sé",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            var existingCep = new Cep
            {
                Id = 1,
                Codigo = "01001000",
                Logradouro = "Praça da Sé"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                          .ReturnsAsync(existingCep);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Cep>()))
                          .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(1, cepDto);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Cep>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteCep()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1))
                          .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
