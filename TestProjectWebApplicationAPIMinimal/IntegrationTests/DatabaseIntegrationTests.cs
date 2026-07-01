// IntegrationTests/DatabaseIntegrationTests.cs
using WebApplicationAPIMinimalNetCoreTreinamento.Data;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using TestProjectWebApplicationAPIMinimal.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CepCrud.Tests.IntegrationTests;

public class DatabaseIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public DatabaseIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DbContext_ShouldBeAbleToAddAndRetrieveCep()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Arrange
        var cep = TestDataFactory.CreateValidCep();

        // Act
        dbContext.Ceps.Add(cep);
        await dbContext.SaveChangesAsync();

        // Assert
        var retrievedCep = await dbContext.Ceps.FindAsync(cep.Codigo);
        retrievedCep.Should().NotBeNull();
        retrievedCep!.Codigo.Should().Be(cep.Codigo);
        retrievedCep.Logradouro.Should().Be(cep.Logradouro);
    }

    [Fact]
    public async Task DbContext_ShouldPreventDuplicateCep()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Arrange
        var cep1 = TestDataFactory.CreateValidCep();
        var cep2 = TestDataFactory.CreateValidCep();
        cep2.Codigo = cep1.Codigo; // Same CEP number

        // Act & Assert
        dbContext.Ceps.Add(cep1);
        await dbContext.SaveChangesAsync();

        // This should throw due to unique constraint
        dbContext.Ceps.Add(cep2);
        var exception = await Record.ExceptionAsync(async () =>
            await dbContext.SaveChangesAsync());

        exception.Should().NotBeNull();
        exception.Should().BeOfType<DbUpdateException>();
    }

    [Fact]
    public async Task DbContext_ShouldBeAbleToUpdateCep()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Arrange
        var cep = TestDataFactory.CreateValidCep();
        dbContext.Ceps.Add(cep);
        await dbContext.SaveChangesAsync();

        // Act
        var retrievedCep = await dbContext.Ceps.FindAsync(cep.Codigo);
        retrievedCep.Should().NotBeNull();
        retrievedCep!.Logradouro = "Rua Atualizada";
        retrievedCep.Numero = "999";
        await dbContext.SaveChangesAsync();

        // Assert
        var updatedCep = await dbContext.Ceps.FindAsync(cep.Codigo);
        updatedCep.Should().NotBeNull();
        updatedCep!.Logradouro.Should().Be("Rua Atualizada");
        updatedCep.Numero.Should().Be("999");
    }

    [Fact]
    public async Task DbContext_ShouldBeAbleToDeleteCep()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Arrange
        var cep = TestDataFactory.CreateValidCep();
        dbContext.Ceps.Add(cep);
        await dbContext.SaveChangesAsync();

        // Act
        var retrievedCep = await dbContext.Ceps.FindAsync(cep.Codigo);
        retrievedCep.Should().NotBeNull();
        dbContext.Ceps.Remove(retrievedCep!);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedCep = await dbContext.Ceps.FindAsync(cep.Codigo);
        deletedCep.Should().BeNull();
    }

    [Fact]
    public async Task DbContext_ShouldBeAbleToQueryByCity()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Arrange
        var ceps = TestDataFactory.CreateCeps(3);
        ceps[0].Cidade = "São Paulo";
        ceps[1].Cidade = "São Paulo";
        ceps[2].Cidade = "Rio de Janeiro";

        dbContext.Ceps.AddRange(ceps);
        await dbContext.SaveChangesAsync();

        // Act
        var spCeps = await dbContext.Ceps
            .Where(c => c.Cidade == "São Paulo")
            .ToListAsync();

        // Assert
        spCeps.Should().HaveCount(2);
        spCeps.Should().AllSatisfy(c => c.Cidade.Should().Be("São Paulo"));
    }
}