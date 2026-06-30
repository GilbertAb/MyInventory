using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.InputModels;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer;
using External.MyInventoryApi.DataAccess.Contracts.SqlServer.Results;
using External.MyInventoryApi.DataAccess.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.DataAccess
{
    public class MovementRepositoryTests
    {
        private readonly Mock<ISqlServerDatabase> _databaseMock;
        private readonly Mock<ILogger<MovementRepository>> _loggerMock;
        private readonly MovementRepository _repository;
        Dictionary<string, string?> configurationData = new Dictionary<string, string?>
        {
            ["StoredProcedures:SP_GET_MOVEMENTS"] = "MyInventory.usp_GetMovements",
            ["StoredProcedures:SP_REGISTER_PRODUCT_MOVEMENT"] = "MyInventory.usp_RegisterProductMovement",
            ["StoredProcedures:SP_GET_PRODUCT_STOCK_HISTORY"] = "MyInventory.usp_GetProductStockHistory"
        };

        public MovementRepositoryTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            _databaseMock = new Mock<ISqlServerDatabase>();
            _loggerMock = new Mock<ILogger<MovementRepository>>();
            _repository = CreateRepository(_databaseMock.Object, configuration, _loggerMock.Object);
        }

        private MovementRepository CreateRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<MovementRepository> logger)
        {
            MovementRepository repository = new MovementRepository(_databaseMock.Object, configuration, _loggerMock.Object);
            return repository;
        }

        /*
         *---------------------------------------------------------
         *---------------| GetMovements use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetMovements_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("MovementTypeId", typeof(int));
            table.Columns.Add("MovementDate", typeof(DateTime));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("MovementDescription", typeof(string));
            table.Columns.Add("CreatedAt", typeof(DateTime));

            table.Rows.Add(1, 12, 1, DateTime.Now, 5, "New products", DateTime.Now);
            table.Rows.Add(2, 27, 1, DateTime.Now, 5, "DAS", DateTime.Now);

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 0,
                    ErrorMessage = string.Empty,
                    Data = dataSet
                });

            // Act
            var result = await _repository.GetMovements();

            // Assert
            result.ErrorCode.Should().Be(0);

            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.First().Id.Should().Be(1);
            result.Data.First().ProductId.Should().Be(12);
            result.Data.First().MovementTypeId.Should().Be(1);
        }


        [Fact]
        public async Task GetMovements_ShouldReturnError_WhenStoredProcedureFails()
        {
            // Arrange

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 500,
                    ErrorMessage = "Database error"
                });

            // Act
            var result = await _repository.GetMovements();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetMovements_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing get movements"));

            // Act
            Func<Task> act = async () =>
                await _repository.GetMovements();

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing get movements");
        }

        /*
         *---------------------------------------------------------
         *---------------| RegisterMovement use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task RegisterProductMovement_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Func<Task> act = async () => await _repository.RegisterProductMovement(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task RegisterProductMovement_ShouldCallStoredProcedureWithCorrectParameters()
        {
            // Arrange
            var movement = new RegisterProductMovementInputModel
            {
                ProductId = 12,
                MovementTypeId = 2,
                Quantity = 15,
                MovementDescription = "Test"
            };

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 0,
                    Data = new DataSet()
                });

            // Act
            await _repository.RegisterProductMovement(movement);

            // Assert
            _databaseMock.Verify(
                d => d.ExecuteAsync(
                    "MyInventory.usp_RegisterProductMovement",
                    It.Is<Dictionary<string, object>>(p =>
                        (int)p["@ProductId"] == 12 &&
                        (byte)p["@MovementTypeId"] == 2 &&
                        (int)p["@Quantity"] == 15 &&
                        (string)p["@MovementDescription"] == "Test"
                    )
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task RegisterProductMovement_ShouldThrowArgumentNullException_WhenProductInputIsNull()
        {
            // Act
            Func<Task> act = () => _repository.RegisterProductMovement(null!);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task RegisterProductMovement_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var movement = new RegisterProductMovementInputModel
            {
                ProductId = 12,
                MovementTypeId = 2,
                Quantity = 15,
                MovementDescription = "Test"
            };

            var dataSet = new DataSet();

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 0,
                    ErrorMessage = string.Empty,
                    Data = dataSet
                });

            // Act
            var result = await _repository.RegisterProductMovement(movement);

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public async Task RegisterProductMovement_ShouldReturnError_WhenStoredProcedureFails()
        {
            // Arrange
            var movement = new RegisterProductMovementInputModel
            {
                ProductId = 12,
                MovementTypeId = 2,
                Quantity = 15,
                MovementDescription = "Test"
            };

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 500,
                    ErrorMessage = "Database error"
                });

            // Act
            var result = await _repository.RegisterProductMovement(movement);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task RegisterProductMovement_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            var movement = new RegisterProductMovementInputModel
            {
                ProductId = 12,
                MovementTypeId = 2,
                Quantity = 15,
                MovementDescription = "Test"
            };

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing registerProductMovement: {movement.ProductId}"));

            // Act
            Func<Task> act = async () =>
                await _repository.RegisterProductMovement(movement);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing registerProductMovement: {movement.ProductId}");
        }

        /*
         *---------------------------------------------------------
         *---------------| GetProductStockHistory use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetProductStockHistory_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("MovementId", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("MovementTypeId", typeof(int));
            table.Columns.Add("MovementDate", typeof(DateTime));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("MovementDescription", typeof(string));
            table.Columns.Add("CreatedAt", typeof(DateTime));

            table.Rows.Add(1, 12, 1, DateTime.Now, 5, "New products", DateTime.Now);
            table.Rows.Add(2, 12, 1, DateTime.Now, 5, "DAS", DateTime.Now);

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 0,
                    ErrorMessage = string.Empty,
                    Data = dataSet
                });

            // Act
            var result = await _repository.GetProductStockHistory(12);

            // Assert
            result.ErrorCode.Should().Be(0);

            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.First().Id.Should().Be(1);
            result.Data.First().ProductId.Should().Be(12);
            result.Data.First().MovementTypeId.Should().Be(1);
            result.Data.Last().Id.Should().Be(2);
            result.Data.Last().ProductId.Should().Be(12);
        }


        [Fact]
        public async Task GetProductStockHistory_ShouldReturnError_WhenStoredProcedureFails()
        {
            // Arrange

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new StoredProcedureResult<DataSet>
                {
                    ErrorCode = 500,
                    ErrorMessage = "Database error"
                });

            // Act
            var result = await _repository.GetProductStockHistory(12);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetProductStockHistory_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing getProductStockHistory"));

            // Act
            Func<Task> act = async () =>
                await _repository.GetProductStockHistory(12);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing getProductStockHistory");
        }
     
    }
}
