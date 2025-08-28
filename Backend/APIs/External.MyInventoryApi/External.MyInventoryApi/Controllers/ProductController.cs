using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Response;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Mappers;
using External.MyInventoryApi.Requests.Product;
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
        
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest product)
        {
            if (product == null)
            {
                return BadRequest("Product can't be null");
            }
            // Call add product service
            ServiceResult<AddProductResponseDto> result = await _productService.AddProduct(
                ProductRequestMapper.MapAddProductRequestToProductDto(product)
            );

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }

        [HttpDelete("deleteProduct/{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            // Call delete product service
            ServiceResult<DeleteProductResponseDto> result = await _productService.DeleteProduct(productId);

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
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

        [HttpGet("getProduct/{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            // Call get product by Id service
            ServiceResult<ProductDto?> result = await _productService.GetProductById(productId);

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
        [HttpGet("getProductStockSummary/{productId:int}")]
        public async Task<IActionResult> GetProductStockSummary(int productId)
        {
            // Call get product by Id service
            ServiceResult<GetProductStockSummaryResponseDto?> result = await _productService.GetProductStockSummary(productId);

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest product)
        {
            if (product == null)
            {
                return BadRequest("Product can't be null");
            }
            // Call add product service
            ServiceResult<UpdateProductResponseDto> result = await _productService.UpdateProduct(
                ProductRequestMapper.MapUpdateProductRequestToProductDto(product)
            );

            if (result.ErrorCode != 0)
            {
                return BadRequest(new { result.ErrorCode, result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
