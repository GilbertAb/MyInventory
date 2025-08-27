using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.Application.Contracts.DTOs.Response.Movement;
using External.MyInventoryApi.Application.Contracts.Results;

namespace External.MyInventoryApi.Application.Contracts.Services
{
    public interface IMovementService
    {
        public Task<ServiceResult<RegisterMovementResponseDto>> RegisterMovement(RegisterMovementRequest request);
        // Get the movements of a product
        public Task<ServiceResult<IEnumerable<MovementDto>?>> GetProductStockHistory(int productId);
    }
}
