// IntegrationTests/CustomWebApplicationFactory.cs
using WebApplicationAPIMinimalNetCoreTreinamento.Data;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using TestProjectWebApplicationAPIMinimal.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CepCrud.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Add InMemory Database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("MeuBancoDeDados");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed test data
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            SeedTestData(db);
        });

        return base.CreateHost(builder);
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        var ceps = TestDataFactory.CreateCeps(5);
        context.Ceps.AddRange(ceps);
        context.SaveChanges();
    }
}
