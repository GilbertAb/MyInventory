using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.Application.Contracts.DTOs.Response;
using External.MyInventoryApi.Application.Contracts.DTOs.Response.Movement;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Mappers;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.Application.Services
{
    public class MovementService : IMovementService
    {
        private readonly IMovementRepository _repository;

        public MovementService(IMovementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ServiceResult<RegisterMovementResponseDto>> RegisterMovement(RegisterMovementRequest request)
        {
            // Validate request
            if (request == null)
            {
                return new ServiceResult<RegisterMovementResponseDto>
                {
                    Data = null,
                    ErrorCode = -1,
                    ErrorMessage = "Request can't be null"
                };
            }

            // Execute add product
            OperationResult<int?> result = await _repository.RegisterProductMovement(
                MovementMapper.MapRegisterMovementRequestToInputModel(request)
            );

            //Map to service result
            ServiceResult<RegisterMovementResponseDto?> serviceResult = 
                OperationResultMapper<RegisterMovementResponseDto, int?>.MapToServiceResult(
                    result,
                    id => new RegisterMovementResponseDto { Id = id }
            );

            return serviceResult!;
        }
    }
}
