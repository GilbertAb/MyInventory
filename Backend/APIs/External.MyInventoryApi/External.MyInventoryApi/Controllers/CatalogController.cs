using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace External.MyInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogService _catalogService;

        public CatalogController(ILogger<CatalogController> logger, ICatalogService catalogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        [HttpGet("getMovementTypes")]
        public async Task<IActionResult> GetMovementTypes()
        {
            // Call getMovementTypes 
            ServiceResult<IEnumerable<MovementTypeDto>?> result = await _catalogService.GetMovementTypes();

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
