using External.MyInventoryApi.DataAccess.Contracts.Results;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Mappers
{
    public static class StoredProcedureResultMapper<T>
    {
        public static OperationResult<T> MapToOperationResult(StoredProcedureResult<DataSet> spResult, Func<DataSet, T> mapFunc)
        {
            return new OperationResult<T>
            {
                Data = spResult.ErrorCode == 0 && spResult.Data != null
                    ? mapFunc(spResult.Data)
                    : default!,
                ErrorCode = spResult.ErrorCode,
                ErrorMessage = spResult.ErrorMessage,
            };
        }
    }
}
