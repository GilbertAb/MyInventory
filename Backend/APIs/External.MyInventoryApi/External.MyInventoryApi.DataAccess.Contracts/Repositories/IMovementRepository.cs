using External.MyInventoryApi.DataAccess.Contracts.InputModels;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.DataAccess.Contracts.Repositories
{
    public interface IMovementRepository
    {
        Task<OperationResult<int?>> RegisterProductMovement(RegisterProductMovementInputModel registerMovementModel);
    }
}
