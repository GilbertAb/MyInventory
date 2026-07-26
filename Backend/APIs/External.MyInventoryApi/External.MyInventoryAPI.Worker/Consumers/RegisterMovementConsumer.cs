using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Business.Messaging.Commands;
using External.MyInventoryAPI.Worker.Mappers;
using MassTransit;

namespace External.MyInventoryAPI.Worker.Consumers
{
    public sealed class RegisterMovementConsumer : IConsumer<RegisterMovementCommand>
    {
        private readonly IMovementService _movementService;
        private readonly ILogger<RegisterMovementConsumer> _logger;

        public RegisterMovementConsumer(IMovementService movementService, ILogger<RegisterMovementConsumer> logger)
        {
            _movementService = movementService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RegisterMovementCommand> context)
        {
            _logger.LogInformation(
                "Processing RegisterMovementCommand for Product {ProductId}",
                context.Message.ProductId);

            try
            {
                await _movementService.RegisterMovement(MovementMapper.MapRegisterMovementCommandToRequest(context.Message));

                _logger.LogInformation("Movement successfully registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing RegisterMovementCommand.");

                throw;
            }
        }
    }
}
