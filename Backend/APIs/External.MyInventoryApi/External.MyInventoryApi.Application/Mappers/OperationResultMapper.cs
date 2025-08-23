using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;

namespace External.MyInventoryApi.Application.Mappers
{
    public static class OperationResultMapper<TService, TRepository>
    {
        public static ServiceResult<TService?> MapToServiceResult(StoredProcedureResult<TRepository> operationResult,
            Func<TRepository, TService> mapFunc)
        {
            return new ServiceResult<TService?>
            {
                Data = operationResult.Data != null
                    ? mapFunc(operationResult.Data)
                    : default,
                ErrorCode = operationResult.ErrorCode,
                ErrorMessage = operationResult.ErrorMessage,
            };
        }
    }
}
