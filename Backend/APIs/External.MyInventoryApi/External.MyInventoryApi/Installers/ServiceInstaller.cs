using External.MyInventoryApi.Application.Contracts.Messaging;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.CrossCutting.Contracts;
using External.MyInventoryApi.CrossCutting.Crypto;
using External.MyInventoryApi.CrossCutting.Messaging;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.Repositories;
using External.MyInventoryApi.DataAccess.SqlServer;
using MassTransit;

namespace External.MyInventoryApi.Installers
{
    public static class ServiceInstaller
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ISqlServerDatabase, SqlServerDatabase>();
            services.AddSingleton<ICrypto, Crypto>();
            // Repositories
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            // Services
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IMovementService, MovementService>();
            services.AddScoped<IProductService, ProductService>();
            // Messaging
            services.AddMassTransit();
            services.AddScoped<IEventBus, RabbitMqEventBus>();
        }

        public static void RegisterMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    ConfigureRabbitMqHost(rabbitMqConfig, configuration);
                });
            });
        }

        private static void ConfigureRabbitMqHost(IRabbitMqBusFactoryConfigurator RabbitMqConfig, IConfiguration configuration)
        {
            var host = configuration["RabbitMQ:Host"];
            var username = configuration["RabbitMQ:Username"];
            var password = configuration["RabbitMQ:Password"];

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "RabbitMQ configuration is missing.");
            }

            RabbitMqConfig.Host(host, h =>
            {
                h.Username(username);
                h.Password(password);
            });
        }
    }
}
