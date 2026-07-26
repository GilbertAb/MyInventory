using External.MyInventoryAPI.Worker;
using External.MyInventoryAPI.Worker.Installers;

var builder = Host.CreateApplicationBuilder(args);

// Configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.RegisterServices();
builder.Services.RegisterMassTransit(builder.Configuration);

var host = builder.Build();
host.Run();
