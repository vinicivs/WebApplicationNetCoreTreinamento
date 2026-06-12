using Microsoft.EntityFrameworkCore;
using WebApplicationRazorNetCoreTreinamento.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //Tipo Razor
builder.Services.AddRazorPages();

// Configuração do DbContext para usar InMemoryDatabase, ou seja, quando o app é fechado se perde os dados de banco de dados, serve somente para prototipos rápidos ou SQL Server
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("DefaultConnection")); // ou UseSqlServer()

// DbContext + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();// Exige que o site seja acessado apenas por HTTPS, aumentando a segurança. Em produção, é recomendado usar HSTS para proteger contra ataques de downgrade e cookie hijacking.
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
