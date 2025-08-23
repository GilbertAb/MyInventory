using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Contracts.SqlServer
{
    public interface ISqlServerDatabase : IDisposable
    {
        // Execute
        Task<StoredProcedureResult<DataSet>> ExecuteAsync(string storedProcedureName, Dictionary<string, object> parameters);

        // Health
        Task<bool> IsHealthyAsync();
    }
}
