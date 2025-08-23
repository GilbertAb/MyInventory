using External.MyInventoryApi.CrossCutting.Contracts;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;


namespace External.MyInventoryApi.DataAccess.SqlServer
{
    public class SqlServerDatabase : ISqlServerDatabase
    {
        private SqlConnection _connection;
        private readonly ICrypto _crypto;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqlServerDatabase> _logger;

        public SqlServerDatabase(ICrypto crypto, IConfiguration configuration, ILogger<SqlServerDatabase> logger)
        {
            _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Connection
            string encryptedConnectionString = _configuration.GetSection("ConnectionStrings:SqlServer").Value
                ?? throw new ArgumentNullException(nameof(encryptedConnectionString));
            string connectionString = _crypto.Decrypt(encryptedConnectionString);

            _connection = new SqlConnection(connectionString);
        }

        // Connection
        private void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }
        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _logger.LogInformation("SQL Server connection closed");
            }
        }

        // Command
        private SqlCommand CreateCommand(string storedProcedureName)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, _connection);

            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        // Execute
        public async Task<StoredProcedureResult<DataSet>> ExecuteAsync(string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                // Open connection
                OpenConnection();

                // Create command
                using SqlCommand command = CreateCommand(storedProcedureName);

                // Add parameters to command
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                // Execute
                using SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet resultDataSet = new DataSet();
                adapter.Fill(resultDataSet);

                if (resultDataSet.Tables.Count < 2)
                {
                    throw new InvalidOperationException(
                        $"Stored procedure {storedProcedureName} did not return both Data and Error resultsets.");
                }

                // Data
                DataTable data = resultDataSet.Tables[0];
                DataSet dataDataSet = new DataSet();
                dataDataSet.Tables.Add(data.Copy());

                // Error
                int errorCode = 0;
                string errorMessage = "OK";
                DataTable errorTable = resultDataSet.Tables[1];

                if (errorTable.Rows.Count > 0)
                {
                    DataRow errorRow = errorTable.Rows[0];

                    errorCode = errorRow["ErrorCode"] != DBNull.Value
                        ? Convert.ToInt32(errorTable.Rows[0]["ErrorCode"])
                        : 0;

                    errorMessage = errorRow["ErrorMessage"] != DBNull.Value
                        ? errorRow["ErrorMessage"].ToString()!
                        : "OK";
                }

                return new StoredProcedureResult<DataSet>
                {
                    Data = dataDataSet,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing stored procedure {StoredProcedureName}", storedProcedureName);

                return new StoredProcedureResult<DataSet>
                {
                    Data = null,
                    ErrorCode = -1,
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                CloseConnection();
            }
        }

        // Health
        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                OpenConnection();

                using var cmd = new SqlCommand("SELECT 1", _connection);
                cmd.ExecuteScalar();

                CloseConnection();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't verify connection to SQL Server");
                return false;
            }
        }

        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
        }
    }
}
