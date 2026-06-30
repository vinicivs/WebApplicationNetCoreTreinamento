using WebApplicationAPIMinimalNetCoreTreinamento.Data;
using WebApplicationAPIMinimalNetCoreTreinamento.DTOs;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CRUD Endpoints

// GET: Listar todos os CEPs
app.MapGet("/api/ceps", async (ApplicationDbContext context) =>
{
    var ceps = await context.Ceps.ToListAsync();
    return Results.Ok(ceps.Select(c => new CepDto
    {
        Id = c.Id,
        Codigo = c.Codigo,
        Logradouro = c.Logradouro,
        Numero = c.Numero,
        Complemento = c.Complemento,
        Bairro = c.Bairro,
        Cidade = c.Cidade,
        Uf = c.Uf
    }));
})
.WithName("GetAllCeps")
.WithOpenApi();

// GET: Buscar CEP por código
app.MapGet("/api/ceps/{codigo:int}", async (int codigo, ApplicationDbContext context) =>
{
    var cep = await context.Ceps.FindAsync(codigo);

    if (cep is null)
        return Results.NotFound($"CEP com código {codigo} não encontrado.");

    return Results.Ok(new CepDto
    {
        Id = cep.Id,
        Codigo = cep.Codigo,
        Logradouro = cep.Logradouro,
        Numero = cep.Numero,
        Complemento = cep.Complemento,
        Bairro = cep.Bairro,
        Cidade = cep.Cidade,
        Uf = cep.Uf
    });
})
.WithName("GetCepById")
.WithOpenApi();

// GET: Buscar CEP por número
app.MapGet("/api/ceps/cep/{codigo}", async (string cepNumero, ApplicationDbContext context) =>
{
    var cep = await context.Ceps
        .FirstOrDefaultAsync(c => c.Codigo == cepNumero);

    if (cep is null)
        return Results.NotFound($"CEP {cepNumero} não encontrado.");

    return Results.Ok(new CepDto
    {
        Id = cep.Id,
        Codigo = cep.Codigo,
        Logradouro = cep.Logradouro,
        Numero = cep.Numero,
        Complemento = cep.Complemento,
        Bairro = cep.Bairro,
        Cidade = cep.Cidade,
        Uf = cep.Uf
    });
})
.WithName("GetCepByNumero")
.WithOpenApi();

// GET: Buscar CEPs por cidade/UF
app.MapGet("/api/ceps/busca", async (string? cidade, string? uf, ApplicationDbContext context) =>
{
    var query = context.Ceps.AsQueryable();

    if (!string.IsNullOrEmpty(cidade))
        query = query.Where(c => c.Cidade.Contains(cidade));

    if (!string.IsNullOrEmpty(uf))
        query = query.Where(c => c.Uf == uf.ToUpper());

    var ceps = await query.ToListAsync();

    return Results.Ok(ceps.Select(c => new CepDto
    {
        Id = c.Id,
        Codigo = c.Codigo,
        Logradouro = c.Logradouro,
        Numero = c.Numero,
        Complemento = c.Complemento,
        Bairro = c.Bairro,
        Cidade = c.Cidade,
        Uf = c.Uf
    }));
})
.WithName("BuscarCeps")
.WithOpenApi();

// POST: Criar novo CEP
app.MapPost("/api/ceps", async (CepCreateDto createDto, ApplicationDbContext context) =>
{
    // Validação básica
    if (string.IsNullOrWhiteSpace(createDto.Codigo))
        return Results.BadRequest("Número do CEP é obrigatório.");

    if (string.IsNullOrWhiteSpace(createDto.Logradouro))
        return Results.BadRequest("Logradouro é obrigatório.");

    // Verifica se o CEP já existe
    var exists = await context.Ceps.AnyAsync(c => c.Codigo == createDto.Codigo);
    if (exists)
        return Results.Conflict($"CEP {createDto.Codigo} já cadastrado.");

    var newCep = new Cep
    {
        Codigo = createDto.Codigo,
        Logradouro = createDto.Logradouro,
        Numero = createDto.Numero,
        Complemento = createDto.Complemento,
        Bairro = createDto.Bairro,
        Cidade = createDto.Cidade,
        Uf = createDto.Uf.ToUpper()
    };

    context.Ceps.Add(newCep);
    await context.SaveChangesAsync();

    return Results.Created($"/api/ceps/{newCep.Codigo}", new CepDto
    {
        Id = newCep.Id,
        Codigo = newCep.Codigo,
        Logradouro = newCep.Logradouro,
        Numero = newCep.Numero,
        Complemento = newCep.Complemento,
        Bairro = newCep.Bairro,
        Cidade = newCep.Cidade,
        Uf = newCep.Uf
    });
})
.WithName("CreateCep")
.WithOpenApi();

// PUT: Atualizar CEP
app.MapPut("/api/ceps/{codigo:int}", async (int codigo, CepUpdateDto updateDto, ApplicationDbContext context) =>
{
    var cep = await context.Ceps.FindAsync(codigo);

    if (cep is null)
        return Results.NotFound($"CEP com código {codigo} não encontrado.");

    // Validações
    if (string.IsNullOrWhiteSpace(updateDto.Codigo))
        return Results.BadRequest("Número do CEP é obrigatório.");

    if (string.IsNullOrWhiteSpace(updateDto.Logradouro))
        return Results.BadRequest("Logradouro é obrigatório.");

    // Verifica se o novo CEP já existe (se foi alterado)
    if (cep.Codigo != updateDto.Codigo)
    {
        var exists = await context.Ceps
            .AnyAsync(c => c.Codigo == updateDto.Codigo && c.Id != codigo);
        if (exists)
            return Results.Conflict($"CEP {updateDto.Codigo} já cadastrado.");
    }

    cep.Codigo = updateDto.Codigo;
    cep.Logradouro = updateDto.Logradouro;
    cep.Numero = updateDto.Numero;
    cep.Complemento = updateDto.Complemento;
    cep.Bairro = updateDto.Bairro;
    cep.Cidade = updateDto.Cidade;
    cep.Uf = updateDto.Uf.ToUpper();

    await context.SaveChangesAsync();

    return Results.Ok(new CepDto
    {
        Id = cep.Id,
        Codigo = cep.Codigo,
        Logradouro = cep.Logradouro,
        Numero = cep.Numero,
        Complemento = cep.Complemento,
        Bairro = cep.Bairro,
        Cidade = cep.Cidade,
        Uf = cep.Uf
    });
})
.WithName("UpdateCep")
.WithOpenApi();

// DELETE: Excluir CEP
app.MapDelete("/api/ceps/{codigo:int}", async (int codigo, ApplicationDbContext context) =>
{
    var cep = await context.Ceps.FindAsync(codigo);

    if (cep is null)
        return Results.NotFound($"CEP com código {codigo} não encontrado.");

    context.Ceps.Remove(cep);
    await context.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteCep")
.WithOpenApi();

// Configure Database Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();


