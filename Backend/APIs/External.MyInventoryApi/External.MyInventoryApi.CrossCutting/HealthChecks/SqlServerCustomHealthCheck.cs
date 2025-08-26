using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace External.MyInventoryApi.CrossCutting.HealthChecks
{
    public class SqlServerCustomHealthCheck : IHealthCheck
    {
        private readonly ISqlServerDatabase _database;

        public SqlServerCustomHealthCheck(ISqlServerDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                bool isHealthy = await _database.IsHealthyAsync();

                if (!isHealthy)
                {
                    return HealthCheckResult.Unhealthy("SQL Server connection is not healthy");
                }

                return HealthCheckResult.Healthy("SQL Server is reachable");
            } catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQL Server check failed", ex);
            }
        }
    }
}
