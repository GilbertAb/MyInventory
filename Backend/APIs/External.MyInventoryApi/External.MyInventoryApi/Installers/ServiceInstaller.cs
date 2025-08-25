using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.CrossCutting.Contracts;
using External.MyInventoryApi.CrossCutting.Crypto;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.Repositories;
using External.MyInventoryApi.DataAccess.SqlServer;

namespace External.MyInventoryApi.Installers
{
    public static class ServiceInstaller
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ISqlServerDatabase, SqlServerDatabase>();
            services.AddSingleton<ICrypto, Crypto>();
            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICatalogService, CatalogService>();
        }
    }
}
