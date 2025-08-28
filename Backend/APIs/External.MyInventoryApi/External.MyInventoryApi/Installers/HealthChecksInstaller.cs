using External.MyInventoryApi.CrossCutting.HealthChecks;

namespace External.MyInventoryApi.Installers
{
    public static class HealthChecksInstaller
    {
        public static IServiceCollection RegisterHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SqlServerCustomHealthCheck>("SqlServer");

            return services;
        }
    }
}
