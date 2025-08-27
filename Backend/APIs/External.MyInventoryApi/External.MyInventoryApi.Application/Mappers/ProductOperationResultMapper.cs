using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.DTOs.Response;
using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.Application.Mappers
{
    public static class ProductOperationResultMapper
    {
        public static readonly Func<IEnumerable<Product>, IEnumerable<ProductDto>?> MapProducts = products =>
        {
            if (products != null)
            {
                var productDtos = new List<ProductDto>();

                foreach (Product product in products)
                {
                    productDtos.Add
                    (
                        new ProductDto
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Category = product.Category,
                            Stock = product.Stock
                        }
                    );
                }

                return productDtos;
            }
            return null;
        };

        public static readonly Func<Product, ProductDto?> MapProduct = product =>
        {
            if (product != null)
            {
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Stock = product.Stock
                };

                return productDto;
            }
            return null;
        };

        public static readonly Func<ProductStockSummaryResult?, GetProductStockSummaryResponseDto?> MapProductStockSummary = summary =>
        {
            if (summary != null)
            {
                var summaryDto = new GetProductStockSummaryResponseDto
                {
                    ProductName = summary.ProductName,
                    Stock = summary.Stock,
                    NumberOfMovements = summary.NumberOfMovements,
                    NumberOfEntries = summary.NumberOfEntries,
                    NumberOfExits = summary.NumberOfExits,
                    LastMovementDate = summary.LastMovementDate
                };

                return summaryDto;
            }
            return null;
        };
    }
}
