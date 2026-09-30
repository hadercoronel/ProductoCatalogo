using Aplicacion.Servicios;
using Infraestructura.Persistencia;
using Infraestructura.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Controladores
builder.Services.AddControllers();

// 2. Base de Datos MySQL
var connectionString = builder.Configuration.GetConnectionString("ProductCatalog");
builder.Services.AddDbContext<AplicacionDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();

// 3. Documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilitar Swagger siempre en Producción y Desarrollo
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Catálogo v1");
    c.RoutePrefix = "swagger"; // Disponible en /swagger
});

// Redirigir la raíz (/) directamente a Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();