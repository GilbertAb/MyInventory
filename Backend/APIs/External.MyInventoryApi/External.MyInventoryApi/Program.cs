
using External.MyInventoryApi.CrossCutting.Middleware;
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

// Middleware
app.UseMiddleware<HeaderValidationMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();
