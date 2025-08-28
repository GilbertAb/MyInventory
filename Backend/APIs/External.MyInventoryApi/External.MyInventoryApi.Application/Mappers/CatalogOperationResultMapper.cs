using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Business.Catalogs;

namespace External.MyInventoryApi.Application.Mappers
{
    // Map operation result's entities to DTOs
    public class CatalogOperationResultMapper
    {
        public static readonly Func<IEnumerable<MovementType>?, IEnumerable<MovementTypeDto>?> MapMovementTypes = movementTypes =>
        {
            if (movementTypes != null)
            {
                var movementTypeDtos = new List<MovementTypeDto>();

                foreach (MovementType type in movementTypes)
                {
                    movementTypeDtos.Add
                    (
                        new MovementTypeDto
                        {
                            Id = type.Id,
                            Type = type.Type,
                        }
                    );
                }

                return movementTypeDtos;
            }
            return null;
        };
    }
}
