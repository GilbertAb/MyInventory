using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.InputModels;
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
    public class MovementRepository : IMovementRepository
    {
        private readonly ISqlServerDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MovementRepository> _logger;
        private readonly string _spRegisterProductMovement;
        private readonly string _spGetProductStockHistory;

        public MovementRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<MovementRepository> logger)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _spRegisterProductMovement = _configuration.GetSection("StoredProcedures:SP_REGISTER_PRODUCT_MOVEMENT").Value
                ?? throw new ArgumentNullException("name of sp registerProductMovement not found");
            _spGetProductStockHistory = _configuration.GetSection("StoredProcedures:SP_GET_PRODUCT_STOCK_HISTORY").Value
                ?? throw new ArgumentNullException("name of sp getProductStockHistory not found");
        }
        public async Task<OperationResult<int?>> RegisterProductMovement(RegisterProductMovementInputModel registerMovementModel)
        {
            if (registerMovementModel is null)
                throw new ArgumentNullException(nameof(registerMovementModel));

            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductId"] = registerMovementModel.ProductId,
                    ["@MovementTypeId"] = registerMovementModel.MovementTypeId,
                    ["@Quantity"] = registerMovementModel.Quantity,
                    ["@MovementDescription"] = registerMovementModel.MovementDescription
                };

                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spRegisterProductMovement, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Movement registered successfully: {ProductId}", registerMovementModel.ProductId);
                }
                else
                {
                    _logger.LogWarning("Error registering product {ProductId}: {ErrorCode} {ErrorMessage}",
                        registerMovementModel.ProductId, spResult.ErrorCode, spResult.ErrorMessage);

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
                    dataDS => MovementStoredProcedureMapper.MapRegisterMovement(dataDS)
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing registerProductMovement: {ProductId}", registerMovementModel.ProductId);
                throw;
            }
        }
        // Get all movements of a product
        public async Task<OperationResult<IEnumerable<Movement>?>> GetProductStockHistory(int productId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["@ProductId"] = productId,
                };
                // Execute SP 
                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spGetProductStockHistory, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Product's movements retrieved successfully");
                }
                else
                {
                    _logger.LogWarning("Error retrieving product's movements: {ErrorCode} {ErrorMessage}",
                        spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<IEnumerable<Movement>?>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<IEnumerable<Movement>?> result = StoredProcedureResultMapper<IEnumerable<Movement>?>
                    .MapToOperationResult(
                        spResult,
                        dataDS => MovementStoredProcedureMapper.MapGetProductStockHistory(dataDS)
                    );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing getProductStockHistory");
                throw;
            }
        }
    }
}
