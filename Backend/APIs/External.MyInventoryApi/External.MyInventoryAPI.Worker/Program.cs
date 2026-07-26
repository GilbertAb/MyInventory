using External.MyInventoryAPI.Worker;
using External.MyInventoryAPI.Worker.Installers;

var builder = Host.CreateApplicationBuilder(args);

// Configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.RegisterMassTransit(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
