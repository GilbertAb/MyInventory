
using External.MyInventoryApi.CrossCutting.Middleware;
using External.MyInventoryApi.Installers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterServices();
builder.Services.RegisterHealthChecks();

// Build
var app = builder.Build();

// Middleware
app.UseMiddleware<HeaderValidationMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
// Http request metrics
app.UseHttpMetrics();

// Endpoints
app.MapControllers();
app.UseMetricServer("/metrics");
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(x => new {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            }),
            Duration = report.TotalDuration.ToString()
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.Run();
