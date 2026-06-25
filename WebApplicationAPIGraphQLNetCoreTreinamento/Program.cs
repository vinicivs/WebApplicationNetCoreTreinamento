using Microsoft.EntityFrameworkCore;
using WebApplicationAPIGraphQLNetCoreTreinamento.Data;
using WebApplicationAPIGraphQLNetCoreTreinamento.GraphQL;
using WebApplicationAPIGraphQLNetCoreTreinamento.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência do repositório
builder.Services.AddScoped<ICepRepository, CepRepository>();

// Configuração do GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<CepQueries>()
    .AddTypeExtension<CepMutations>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Configuração do GraphQL
app.MapGraphQL("/graphql");

// Executar migrations e seed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
