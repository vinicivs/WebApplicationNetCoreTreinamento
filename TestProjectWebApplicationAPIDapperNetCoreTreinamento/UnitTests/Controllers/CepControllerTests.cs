using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPIDapperNetCoreTreinamento.Controllers;
using WebApplicationAPIDapperNetCoreTreinamento.DTOs;
using WebApplicationAPIDapperNetCoreTreinamento.Services;

namespace TestProjectWebApplicationAPIDapperNetCoreTreinamento.UnitTests.Controllers
{
    public class CepControllerTests
    {
        private readonly Mock<ICepService> _mockService;
        private readonly CepController _controller;

        public CepControllerTests()
        {
            _mockService = new Mock<ICepService>();
            _controller = new CepController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithCeps()
        {
            // Arrange
            var ceps = new List<CepResponseDto>
        {
            new() { Id = 1, Codigo = "01001000", Logradouro = "Praça da Sé" },
            new() { Id = 2, Codigo = "20040002", Logradouro = "Av. Presidente Vargas" }
        };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(ceps);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(ceps);
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOk()
        {
            // Arrange
            var cep = new CepResponseDto
            {
                Id = 1,
                Codigo = "01001000",
                Logradouro = "Praça da Sé"
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cep);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(cep);
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(999))
                       .ReturnsAsync((CepResponseDto?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldReturnCreated()
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

            var createdCep = new CepResponseDto
            {
                Id = 3,
                Codigo = "12345678",
                Logradouro = "Rua Teste",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            _mockService.Setup(s => s.CreateAsync(cepDto)).ReturnsAsync(createdCep);

            // Act
            var result = await _controller.Create(cepDto);

            // Assert
            var createdAtResult = result.Result as CreatedAtActionResult;
            createdAtResult.Should().NotBeNull();
            createdAtResult?.StatusCode.Should().Be(201);
            createdAtResult?.Value.Should().BeEquivalentTo(createdCep);
        }

        [Fact]
        public async Task Create_WithDuplicateCep_ShouldReturnBadRequest()
        {
            // Arrange
            var cepDto = new CepDto { Codigo = "01001000" };

            _mockService.Setup(s => s.CreateAsync(cepDto))
                       .ThrowsAsync(new InvalidOperationException("CEP já cadastrado"));

            // Act
            var result = await _controller.Create(cepDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldReturnNoContent()
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

            _mockService.Setup(s => s.UpdateAsync(1, cepDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, cepDto);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult?.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult?.StatusCode.Should().Be(204);
        }
    }
}
