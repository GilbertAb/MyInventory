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
        private readonly string _spGetAllProducts;

        public ProductRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<ProductRepository> logger)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _spGetAllProducts = _configuration.GetSection("StoredProcedures:SP_GET_PRODUCTS").Value
                ?? throw new ArgumentNullException("name of sp get products not found");
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

    }
}
