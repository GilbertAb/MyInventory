using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Results;

namespace External.MyInventoryApi.Application.Contracts.Services
{
    public interface ICatalogService
    {
        public Task<ServiceResult<IEnumerable<MovementTypeDto>?>> GetMovementTypes();
    }
}
