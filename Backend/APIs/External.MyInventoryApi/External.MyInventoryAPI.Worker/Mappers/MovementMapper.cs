using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.Business.Messaging.Commands;

namespace External.MyInventoryAPI.Worker.Mappers
{
    public static class MovementMapper
    {
        // Map RegisterMovementCommand to RegisterMovementRequest
        public static RegisterMovementRequest MapRegisterMovementCommandToRequest(RegisterMovementCommand command)
        {
            return new RegisterMovementRequest
            {
                ProductId = command.ProductId,
                MovementTypeId = command.MovementTypeId,
                Quantity = command.Quantity,
                MovementDescription = command.MovementDescription,
            };
        }
    }
}
