using EventWebAPI.Data;
using EventWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Eventos.",
        Version = "v1",
        Description = "API para gerenciamento de eventos.",
        Contact = new OpenApiContact
        {
            Name = "João Victor Póvoa França",
            Email = "joaovictorpfr@gmail.com",
            Url = new Uri("https://joaoito-blog.vercel.app/")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
    // Ativar suporte a anotações (como [Display], [Required], etc.)
    c.EnableAnnotations();
});



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

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("A aplicação foi iniciada com sucesso.");
});

app.Lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("A aplicação foi finalizada.");
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
