
using External.MyInventoryApi.Installers;

var builder = WebApplication.CreateBuilder(args);

// Configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterServices();

// Build
var app = builder.Build();

// TODO: Middleware

app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();
