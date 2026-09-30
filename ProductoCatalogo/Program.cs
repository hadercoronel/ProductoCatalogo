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

// 4. Middlewares de Swagger (disponible en Desarrollo y Producción)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();