using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.DataAccess.Contracts.InputModels;

namespace External.MyInventoryApi.Application.Mappers
{
    public class MovementMapper
    {
        // Map RegisterMovementRequest to RegisterProductMovementInputModel
        public static RegisterProductMovementInputModel MapRegisterMovementRequestToInputModel(RegisterMovementRequest request)
        {
            return new RegisterProductMovementInputModel
            {
                ProductId = request.ProductId,
                MovementTypeId = request.MovementTypeId,
                Quantity = request.Quantity,
                MovementDescription = request.MovementDescription,
            };
        }
    }
}
