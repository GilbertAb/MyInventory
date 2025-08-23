using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Response;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Mappers;
using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ServiceResult<AddProductResponseDto>> AddProduct(ProductDto product)
        {
            // Validate product
            if (product == null)
            {
                return new ServiceResult<AddProductResponseDto>
                {
                    Data = null,
                    ErrorCode = -1,
                    ErrorMessage = "Product can't be null"
                };
            }
            // Validate product name
            if (string.IsNullOrEmpty(product.ProductName))
            {
                return new ServiceResult<AddProductResponseDto>
                {
                    Data = null,
                    ErrorCode = -1,
                    ErrorMessage = "Product name can't be null or empty"
                };
            }

            // Execute add product
            OperationResult<int?> result = await _repository.AddProduct(ProductMapper.MapProductDtoToProduct(product));

            //Map to service result
            ServiceResult<AddProductResponseDto?> serviceResult = OperationResultMapper<AddProductResponseDto, int?>.MapToServiceResult(
                result,
                id => new AddProductResponseDto { Id = id }
            );

            return serviceResult!;
        }

        public async Task<ServiceResult<IEnumerable<ProductDto>?>> GetAllProducts()
        {
            // Execute get all products
            OperationResult<IEnumerable<Product>?> result = await _repository.GetAllProducts();

            // Map to Service Result
            ServiceResult<IEnumerable<ProductDto>?> serviceResult =
                OperationResultMapper<IEnumerable<ProductDto>?, IEnumerable<Product>?>
                    .MapToServiceResult(
                        result,
                        products => ProductOperationResultMapper.MapProducts(products)
                );

            return serviceResult;
        }
    }
}
