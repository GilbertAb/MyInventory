using External.MyInventoryApi.Application.Contracts.Messaging;
using External.MyInventoryApi.CrossCutting.Contracts;
using External.MyInventoryApi.CrossCutting.Crypto;
using External.MyInventoryApi.CrossCutting.Messaging;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.SqlServer;
using External.MyInventoryAPI.Worker.Consumers;
using MassTransit;

namespace External.MyInventoryAPI.Worker.Installers
{
    public static class ServiceInstaller
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ISqlServerDatabase, SqlServerDatabase>();
            services.AddSingleton<ICrypto, Crypto>();
            

            // Messaging
            services.AddMassTransit();
            services.AddScoped<IEventBus, RabbitMqEventBus>();
        }

        public static void RegisterMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<RegisterMovementConsumer>();

                x.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    ConfigureRabbitMqHost(rabbitMqConfig, configuration);

                    rabbitMqConfig.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromSeconds(5));
                    });

                    rabbitMqConfig.ConfigureEndpoints(context);
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
