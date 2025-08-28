using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Results;

namespace External.MyInventoryApi.DataAccess.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<OperationResult<int?>> AddProduct(Product product);
        Task<OperationResult<int?>> DeleteProduct(int productId);
        Task<OperationResult<IEnumerable<Product>?>> GetAllProducts();
        Task<OperationResult<Product?>> GetProductById(int productId);
        Task<OperationResult<ProductStockSummaryResult?>> GetProductStockSummary(int productId);
        Task<OperationResult<int?>> UpdateProduct(Product product);
    }
}
