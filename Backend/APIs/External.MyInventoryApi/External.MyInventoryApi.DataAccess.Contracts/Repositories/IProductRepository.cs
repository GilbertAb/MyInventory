using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.DataAccess.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<OperationResult<int?>> AddProduct(Product product);
        Task<OperationResult<int?>> DeleteProduct(int productId);
        Task<OperationResult<IEnumerable<Product>?>> GetAllProducts();
        Task<OperationResult<int?>> UpdateProduct(Product product);
    }
}
