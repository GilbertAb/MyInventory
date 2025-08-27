using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.Application.Contracts.DTOs.Response.Movement;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace External.MyInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementController : ControllerBase
    {
        private readonly ILogger<MovementController> _logger;
        private readonly IMovementService _movementService;

        public MovementController(ILogger<MovementController> logger, IMovementService movementService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movementService = movementService ?? throw new ArgumentNullException(nameof(movementService));
        }

        [HttpPost("registerMovement")]
        public async Task<IActionResult> RegisterMovement([FromBody] RegisterMovementRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request can't be null");
            }
            // Call add product service
            ServiceResult<RegisterMovementResponseDto> result = await _movementService.RegisterMovement(request);

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
        [HttpGet("getProductStockHistory/{productId:int}")]
        public async Task<IActionResult> GetProductStockHistory(int productId)
        {
            // Call getProductStockHistory service
            ServiceResult<IEnumerable<MovementDto>?> result = await _movementService.GetProductStockHistory(productId);

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
