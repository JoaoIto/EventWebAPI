using EventWebAPI.Data;
using EventWebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar o DbContext com banco de dados em memória antes de `builder.Build()`
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EventosDB"));

var app = builder.Build();

// Inserir dados de teste no banco em memória
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Categorias.Add(new Categoria { Nome = "Categoria de Teste" });
    context.SaveChanges();
}

// Configure o pipeline aqui
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
