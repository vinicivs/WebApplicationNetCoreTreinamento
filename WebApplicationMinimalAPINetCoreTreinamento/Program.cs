using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Settings.Configuration;
using System.Reflection;
using System.Text.Json;
using WebApplicationMinimalAPINetCoreTreinamento.API.Health;
using WebApplicationMinimalAPINetCoreTreinamento.API.Middlewares;
using WebApplicationMinimalAPINetCoreTreinamento.Application.Mappings;
using WebApplicationMinimalAPINetCoreTreinamento.Application.Services;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Events;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Interfaces;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Data;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.EventHandlers;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Repositories;
using HealthChecks.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/cep-api-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<CreateCepDtoValidator>();
        fv.AutomaticValidationEnabled = false;
    });

// API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CEP API",
        Version = "v1",
        Description = "API for managing Brazilian ZIP codes (CEP)",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@cepapi.com"
        }
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CepCreatedEvent>();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(CepMappingProfile));

// Kafka Configuration
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IKafkaMessageProcessor, CepMessageProcessor>();
builder.Services.AddHostedService<KafkaConsumerService>();

// Repositories
builder.Services.AddScoped<ICepRepository, CepRepository>();

// Services
builder.Services.AddScoped<ICepService, CepService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Redis Cache (optional)
if (!string.IsNullOrEmpty(builder.Configuration["Redis:ConnectionString"]))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["Redis:ConnectionString"];
        options.InstanceName = "CepApi";
    });
}

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 10;
    });
});

// Distributed Cache
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CEP API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCompression();

app.UseRateLimiter();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Health Check endpoint
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var response = new
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds,
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Duration = e.Value.Duration.TotalMilliseconds,
                Description = e.Value.Description,
                Data = e.Value.Data
            })
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        Log.Information("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Database migration failed");
        throw;
    }
}

try
{
    Log.Information("Starting application");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

//app.Run();

//record Cep(int Id, string Codigo, string Logradouro, string Numero, string Complemento, string Bairro, string Cidade, string Uf);

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
