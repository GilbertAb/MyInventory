using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Requests.Product;

namespace External.MyInventoryApi.Mappers
{
    public static class ProductRequestMapper
    {
        public static ProductDto MapAddProductRequestToProductDto(AddProductRequest request)
        {
            return new ProductDto
            {
                ProductName = request.ProductName,
                Category = request.Category,
                Stock = request.Stock
            };
        }
    }
}
