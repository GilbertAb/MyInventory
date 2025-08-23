using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace External.MyInventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet("getProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            // Call get all products service
            ServiceResult<IEnumerable<ProductDto>?> result = await _productService.GetAllProducts();

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
