using Aplicacion.Servicios;
using Infraestructura.Persistencia;
using Infraestructura.Persistencia.Repositorios;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ProductoCatalogo.Meddlewares;

var builder = WebApplication.CreateBuilder(args);

// 0. Puerto de Railway: la plataforma inyecta $PORT. Escuchar en 0.0.0.0:$PORT.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

// Soporte de proxy inverso de Railway (X-Forwarded-Proto / X-Forwarded-For)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// 1. Configuración de Controladores
builder.Services.AddControllers();

// 2. Base de Datos MySQL con resolución resiliente de connection string.
// Orden: ConnectionStrings__ProductCatalog > ProductCatalog > DATABASE_URL/MYSQL_URL >
// variables MYSQLHOST/MYSQLPORT/MYSQLDATABASE/MYSQLUSER/MYSQLPASSWORD (plugin MySQL de Railway) >
// appsettings.json
static string? BuildFromRailwayMySqlVars()
{
    var host = Environment.GetEnvironmentVariable("MYSQLHOST");
    if (string.IsNullOrWhiteSpace(host)) return null;
    var portEnv = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
    var db = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "";
    var user = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "";
    var pass = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "";
    return $"Server={host};Port={portEnv};Database={db};User={user};Password={pass};AllowPublicKeyRetrieval=True;SslMode=Preferred;";
}

var connectionString = builder.Configuration.GetConnectionString("ProductCatalog")
    ?? builder.Configuration["ProductCatalog"]
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__ProductCatalog")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? Environment.GetEnvironmentVariable("MYSQL_URL")
    ?? BuildFromRailwayMySqlVars();

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("WARNING: no se encontró connection string 'ProductCatalog'. La app arrancará pero los endpoints de BD fallarán hasta configurar la variable.");
}

builder.Services.AddDbContext<AplicacionDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        // Cadena dummy: permite arrancar y pasar el healthcheck aunque no haya BD configurada.
        options.UseMySql("Server=localhost;Port=3306;Database=__nodb__;User=nobody;Password=nobody;",
            new MySqlServerVersion(new Version(8, 0, 36)));
        return;
    }

    // ServerVersion.AutoDetect abre una conexión; si la BD no está disponible en el arranque
    // (típico en Railway), no debemos tumbar el proceso: caer a una versión fija.
    try
    {
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        options.UseMySql(connectionString, serverVersion);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"WARNING: AutoDetect de MySQL falló ({ex.Message}). Usando versión fija 8.0.");
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
    }
});

builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();

// 3. Documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Confiar en cabeceras del proxy antes de cualquier redirección/autenticación
app.UseForwardedHeaders();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Habilitar Swagger siempre en Producción y Desarrollo
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Catálogo v1");
    c.RoutePrefix = "swagger"; // Disponible en /swagger
});

// Healthcheck para Railway (sin depender de la BD)
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// Redirigir la raíz (/) directamente a Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

// En Railway el TLS termina en el proxy; UseHttpsRedirection provocaría
// redirecciones infinitas o fallos. Solo aplica en local.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
