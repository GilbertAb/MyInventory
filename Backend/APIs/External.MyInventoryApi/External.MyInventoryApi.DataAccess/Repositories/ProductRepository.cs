using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;
using External.MyInventoryApi.DataAccess.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace External.MyInventoryApi.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlServerDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductRepository> _logger;
        private readonly string _spAddProduct;
        private readonly string _spDeleteProduct;
        private readonly string _spGetAllProducts;
        private readonly string _spGetProductById;
        private readonly string _spUpdateProduct;

        public ProductRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<ProductRepository> logger)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _spAddProduct = _configuration.GetSection("StoredProcedures:SP_ADD_PRODUCT").Value
                ?? throw new ArgumentNullException("name of sp add product not found");
            _spDeleteProduct = _configuration.GetSection("StoredProcedures:SP_DELETE_PRODUCT").Value
                ?? throw new ArgumentNullException("name of sp add product not found");
            _spGetAllProducts = _configuration.GetSection("StoredProcedures:SP_GET_PRODUCTS").Value
                ?? throw new ArgumentNullException("name of sp get products not found");
            _spGetProductById = _configuration.GetSection("StoredProcedures:SP_GET_PRODUCT_BY_ID").Value
                ?? throw new ArgumentNullException("name of sp get products not found");
            _spUpdateProduct = _configuration.GetSection("StoredProcedures:SP_UPDATE_PRODUCT").Value
                ?? throw new ArgumentNullException("name of sp add product not found");
        }
        public async Task<OperationResult<int?>> AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new ArgumentException("Product name can´t be empty", nameof(product));

            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductName"] = product.ProductName,
                    ["@Category"] = product.Category ?? "",
                    ["@Stock"] = product.Stock
                };

                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spAddProduct, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Product added successfully: {ProductName}", product.ProductName);
                }
                else
                {
                    _logger.LogWarning("Error adding product {ProductName}: {ErrorCode} {ErrorMessage}",
                        product.ProductName, spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<int?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<int?> result = StoredProcedureResultMapper<int?>.MapToOperationResult(
                    spResult,
                    dataDS => ProductStoredProcedureMappers.MapAddProduct(dataDS)
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing add product: {ProductName}", product.ProductName);
                throw;
            }
        }

        public async Task<OperationResult<int?>> UpdateProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new ArgumentException("Product name can´t be empty", nameof(product));

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductId"] = product.Id,
                    ["@ProductName"] = product.ProductName,
                    ["@Category"] = product.Category ?? "",
                    ["@Stock"] = product.Stock
                };

                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spUpdateProduct, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Product updated successfully: {ProductName}", product.ProductName);
                }
                else
                {
                    _logger.LogWarning("Error updating product {ProductName}: {ErrorCode} {ErrorMessage}",
                        product.ProductName, spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<int?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<int?> result = StoredProcedureResultMapper<int?>.MapToOperationResult(
                    spResult,
                    dataDS => ProductStoredProcedureMappers.MapUpdateProduct(dataDS)
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing update product: {ProductName}", product.ProductName);
                throw;
            }
        }

        public async Task<OperationResult<int?>> DeleteProduct(int productId)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductId"] = productId,
                };

                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spDeleteProduct, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Product deleted successfully: {ProductId}", productId);
                }
                else
                {
                    _logger.LogWarning("Error deleting product {ProductId}: {ErrorCode} {ErrorMessage}",
                        productId, spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<int?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<int?> result = StoredProcedureResultMapper<int?>.MapToOperationResult(
                    spResult,
                    dataDS => ProductStoredProcedureMappers.MapDeleteProduct(dataDS)
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing delete product: {ProductId}", productId);
                throw;
            }
        }

        public async Task<OperationResult<IEnumerable<Product>?>> GetAllProducts()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                };
                // Execute SP 
                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spGetAllProducts, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Products retrieved successfully");
                }
                else
                {
                    _logger.LogWarning("Error retrieving products: {ErrorCode} {ErrorMessage}",
                        spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<IEnumerable<Product>?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<IEnumerable<Product>?> result = StoredProcedureResultMapper<IEnumerable<Product>?>
                    .MapToOperationResult(
                        spResult,
                        dataDS => ProductStoredProcedureMappers.MapProducts(dataDS)
                    );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing get products");
                throw;
            }
        }

        public async Task<OperationResult<Product?>> GetProductById(int productId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductId"] = productId,
                };
                // Execute SP 
                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spGetProductById, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Product retrieved successfully");
                }
                else
                {
                    _logger.LogWarning("Error retrieving product: {ErrorCode} {ErrorMessage}",
                        spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<Product?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<Product?> result = StoredProcedureResultMapper<Product?>
                    .MapToOperationResult(
                        spResult,
                        dataDS => ProductStoredProcedureMappers.MapGetProduct(dataDS)
                    );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing get product by id");
                throw;
            }
        }
    }
}
