using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.InputModels;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.DataAccess.Contracts.Repositories
{
    public interface IMovementRepository
    {
        Task<OperationResult<int?>> RegisterProductMovement(RegisterProductMovementInputModel registerMovementModel);
        // Get all movements of a product
        Task<OperationResult<IEnumerable<Movement>?>> GetProductStockHistory(int productId);
    }
}
