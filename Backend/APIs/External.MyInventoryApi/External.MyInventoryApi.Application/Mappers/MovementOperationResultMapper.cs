using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Business.Catalogs;
using External.MyInventoryApi.Business.Entities;

namespace External.MyInventoryApi.Application.Mappers
{
    public class MovementOperationResultMapper
    {
        // Map IEnumerable<Movement> to IEnumerable<MovementDto>
        public static readonly Func<IEnumerable<Movement>?, IEnumerable<MovementDto>?> MapMovements= movements =>
        {
            if (movements != null)
            {
                var movementDtos = new List<MovementDto>();

                foreach (Movement movement in movements)
                {
                    movementDtos.Add
                    (
                        new MovementDto
                        {
                            Id = movement.Id,
                            ProductId = movement.ProductId,
                            MovementTypeId = movement.MovementTypeId,
                            MovementDate = movement.MovementDate,
                            Quantity = movement.Quantity,
                            MovementDescription = movement.MovementDescription
                        }
                    );
                }

                return movementDtos;
            }
            return null;
        };
    }
}
