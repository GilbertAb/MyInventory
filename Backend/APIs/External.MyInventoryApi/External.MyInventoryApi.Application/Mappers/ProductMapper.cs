using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Business.Entities;

namespace External.MyInventoryApi.Application.Mappers
{
    public static class ProductMapper
    {
        public static Product MapProductDtoToProduct(ProductDto product)
        {
            return new Product
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Category = product.Category,
                Stock = product.Stock,
            };
        }
    }
}
