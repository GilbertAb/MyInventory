using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Results;

namespace External.MyInventoryApi.Application.Contracts.Services
{
    public interface IProductService
    {
        public Task<ServiceResult<IEnumerable<ProductDto>?>> GetAllProducts();
    }
}
