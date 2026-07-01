// IntegrationTests/CepControllerIntegrationTests.cs
using WebApplicationAPIMinimalNetCoreTreinamento.DTOs;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using TestProjectWebApplicationAPIMinimal.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using CepCrud.Tests.IntegrationTests;

namespace TestProjectWebApplicationAPIMinimal.IntegrationTests;

public class CepControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public CepControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetAllCeps_ShouldReturnOkAndListCeps()
    {
        // Act
        var response = await _client.GetAsync("/api/ceps");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var ceps = JsonSerializer.Deserialize<List<CepDto>>(content, _jsonOptions);
        ceps.Should().NotBeNull();
        ceps!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetCepById_ShouldReturnOk_WhenCepExists()
    {
        // Act - First get all to find a valid ID
        var getAllResponse = await _client.GetAsync("/api/ceps");
        var content = await getAllResponse.Content.ReadAsStringAsync();
        var ceps = JsonSerializer.Deserialize<List<CepDto>>(content, _jsonOptions);

        if (ceps == null || !ceps.Any())
        {
            // Create a new CEP first
            var createDto = TestDataFactory.CreateValidCepCreateDto();
            var createResponse = await _client.PostAsJsonAsync("/api/ceps", createDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createdCep = JsonSerializer.Deserialize<CepDto>(createContent, _jsonOptions);
            createdCep.Should().NotBeNull();

            // Get the created CEP
            var response = await _client.GetAsync($"/api/ceps/{createdCep!.Codigo}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        else
        {
            // Act
            var cep = ceps.First();
            var response = await _client.GetAsync($"/api/ceps/{cep.Codigo}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var foundCep = JsonSerializer.Deserialize<CepDto>(responseContent, _jsonOptions);
            foundCep.Should().NotBeNull();
            foundCep!.Codigo.Should().Be(cep.Codigo);
        }
    }

    [Fact]
    public async Task GetCepById_ShouldReturnNotFound_WhenCepDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/ceps/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task GetCepByNumero_ShouldReturnOk_WhenCepExists()
    {
        // Act - Get all and get a valid CEP number
        var getAllResponse = await _client.GetAsync("/api/ceps");
        var content = await getAllResponse.Content.ReadAsStringAsync();
        var ceps = JsonSerializer.Deserialize<List<CepDto>>(content, _jsonOptions);

        if (ceps != null && ceps.Any())
        {
            var cepNumber = ceps.First().Codigo;
            var response = await _client.GetAsync($"/api/ceps/cep/{cepNumber}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var foundCep = JsonSerializer.Deserialize<CepDto>(responseContent, _jsonOptions);
            foundCep.Should().NotBeNull();
            foundCep!.Codigo.Should().Be(cepNumber);
        }
    }

    [Fact]
    public async Task GetCepByNumero_ShouldReturnNotFound_WhenCepDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/ceps/cep/00000-000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task BuscarCeps_ShouldReturnFilteredResults()
    {
        // Act - Search by city
        var response = await _client.GetAsync("/api/ceps/busca?cidade=São Paulo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var ceps = JsonSerializer.Deserialize<List<CepDto>>(content, _jsonOptions);
        ceps.Should().NotBeNull();
        if (ceps!.Any())
        {
            ceps.Should().AllSatisfy(c => c.Cidade.Should().Be("São Paulo"));
        }
    }

    [Fact]
    public async Task CreateCep_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var createDto = TestDataFactory.CreateValidCepCreateDto();

        // Act
        var response = await _client.PostAsJsonAsync("/api/ceps", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var createdCep = JsonSerializer.Deserialize<CepDto>(content, _jsonOptions);
        createdCep.Should().NotBeNull();
        createdCep!.Codigo.Should().Be(createDto.Codigo);
        createdCep.Logradouro.Should().Be(createDto.Logradouro);
    }

    [Fact]
    public async Task CreateCep_ShouldReturnConflict_WhenCepAlreadyExists()
    {
        // Arrange - First create a CEP
        var createDto = TestDataFactory.CreateValidCepCreateDto();
        var createResponse = await _client.PostAsJsonAsync("/api/ceps", createDto);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act - Try to create the same CEP
        var duplicateResponse = await _client.PostAsJsonAsync("/api/ceps", createDto);

        // Assert
        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var content = await duplicateResponse.Content.ReadAsStringAsync();
        content.Should().Contain("já cadastrado");
    }

    [Fact]
    public async Task CreateCep_ShouldReturnBadRequest_WhenMissingRequiredFields()
    {
        // Arrange
        var invalidDto = new CepCreateDto
        {
            Codigo = "",
            Logradouro = "",
            Bairro = "Bairro Teste",
            Cidade = "Cidade Teste",
            Uf = "SP"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/ceps", invalidDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("obrigatório");
    }

    [Fact]
    public async Task UpdateCep_ShouldReturnOk_WhenValidData()
    {
        // Arrange - Create a CEP first
        var createDto = TestDataFactory.CreateValidCepCreateDto();
        var createResponse = await _client.PostAsJsonAsync("/api/ceps", createDto);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdCep = JsonSerializer.Deserialize<CepDto>(createContent, _jsonOptions);
        createdCep.Should().NotBeNull();

        // Act - Update the CEP
        var updateDto = TestDataFactory.CreateValidCepUpdateDto();
        updateDto.Codigo = createdCep!.Codigo; // Keep the same CEP number
        var response = await _client.PutAsJsonAsync($"/api/ceps/{createdCep.Codigo}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        var updatedCep = JsonSerializer.Deserialize<CepDto>(responseContent, _jsonOptions);
        updatedCep.Should().NotBeNull();
        updatedCep!.Logradouro.Should().Be(updateDto.Logradouro);
        updatedCep.Bairro.Should().Be(updateDto.Bairro);
    }

    [Fact]
    public async Task UpdateCep_ShouldReturnNotFound_WhenCepDoesNotExist()
    {
        // Arrange
        var updateDto = TestDataFactory.CreateValidCepUpdateDto();

        // Act
        var response = await _client.PutAsJsonAsync("/api/ceps/99999", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task DeleteCep_ShouldReturnNoContent_WhenCepExists()
    {
        // Arrange - Create a CEP first
        var createDto = TestDataFactory.CreateValidCepCreateDto();
        var createResponse = await _client.PostAsJsonAsync("/api/ceps", createDto);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdCep = JsonSerializer.Deserialize<CepDto>(createContent, _jsonOptions);
        createdCep.Should().NotBeNull();

        // Act
        var response = await _client.DeleteAsync($"/api/ceps/{createdCep!.Codigo}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the CEP was deleted
        var getResponse = await _client.GetAsync($"/api/ceps/{createdCep.Codigo}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCep_ShouldReturnNotFound_WhenCepDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/api/ceps/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("não encontrado");
    }
}