using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WebApplicationAPIDapperNetCoreTreinamento.DTOs;

namespace TestProjectWebApplicationAPIDapperNetCoreTreinamento.IntegrationTests
{
    public class CepIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CepIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCeps_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/cep");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var ceps = await response.Content.ReadFromJsonAsync<List<CepResponseDto>>();
            ceps.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateCep_ShouldReturnCreated()
        {
            // Arrange
            var newCep = new CepDto
            {
                Codigo = "87654323",
                Logradouro = "Rua Teste Integração",
                Numero = "123",
                Complemento = "Complemento",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/cep", newCep);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
