using External.MyInventoryApi.Business.Catalogs;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.DataAccess.Contracts.Repositories
{
    public interface ICatalogRepository
    {
        Task<OperationResult<IEnumerable<MovementType>?>> GetMovementTypes();
    }
}
