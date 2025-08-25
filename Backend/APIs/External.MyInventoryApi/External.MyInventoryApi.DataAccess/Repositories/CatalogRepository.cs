using External.MyInventoryApi.Business.Catalogs;
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
    public class CatalogRepository : ICatalogRepository
    {
        private readonly ISqlServerDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CatalogRepository> _logger;
        private readonly string _spGetMovementTypes;

        public CatalogRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<CatalogRepository> logger)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _spGetMovementTypes = _configuration.GetSection("StoredProcedures:SP_GET_MOVEMENT_TYPES").Value
                ?? throw new ArgumentNullException("name of sp getMovementTypes not found");
        }

        public async Task<OperationResult<IEnumerable<MovementType?>>> GetMovementTypes()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                };
                // Execute SP 
                StoredProcedureResult<DataSet> spResult = await _database.ExecuteAsync(_spGetMovementTypes, parameters);

                // Validate result
                if (spResult.ErrorCode == 0)
                {
                    _logger.LogInformation("Movement types retrieved successfully");
                }
                else
                {
                    _logger.LogWarning("Error retrieving movement types: {ErrorCode} {ErrorMessage}",
                        spResult.ErrorCode, spResult.ErrorMessage);

                    return new OperationResult<IEnumerable<MovementType?>>
                    {
                        Data = null,
                        ErrorCode = spResult.ErrorCode,
                        ErrorMessage = spResult.ErrorMessage
                    };
                }

                // Map result
                OperationResult<IEnumerable<MovementType?>> result = StoredProcedureResultMapper<IEnumerable<MovementType?>>
                    .MapToOperationResult(
                        spResult,
                        dataDS => CatalogStoredProcedureMapper.MapGetMovementTypes(dataDS)
                    );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing getMovementTypes");
                throw;
            }
        }
    }
}
