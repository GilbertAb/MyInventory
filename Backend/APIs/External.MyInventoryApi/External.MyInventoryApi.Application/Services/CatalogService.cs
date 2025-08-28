using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Mappers;
using External.MyInventoryApi.Business.Catalogs;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _repository;

        public CatalogService(ICatalogRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ServiceResult<IEnumerable<MovementTypeDto>?>> GetMovementTypes()
        {
            // Execute getMovementTypes
            OperationResult<IEnumerable<MovementType>?> result = await _repository.GetMovementTypes();

            // Map to Service Result
            ServiceResult<IEnumerable<MovementTypeDto>?> serviceResult =
                OperationResultMapper<IEnumerable<MovementTypeDto>?, IEnumerable<MovementType>?>
                    .MapToServiceResult(
                        result,
                        movementTypes => CatalogOperationResultMapper.MapMovementTypes(movementTypes)
                );

            return serviceResult;
        }
    }
}
