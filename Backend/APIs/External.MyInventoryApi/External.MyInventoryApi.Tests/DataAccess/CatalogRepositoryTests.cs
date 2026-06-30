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
    public class CatalogRepositoryTests
    {
        private readonly Mock<ISqlServerDatabase> _databaseMock;
        private readonly Mock<ILogger<CatalogRepository>> _loggerMock;
        private readonly CatalogRepository _repository;
        Dictionary<string, string?> configurationData = new Dictionary<string, string?>
        {
            ["StoredProcedures:SP_GET_MOVEMENT_TYPES"] = "MyInventory.usp_GetMovementTypes"
        };

        public CatalogRepositoryTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            _databaseMock = new Mock<ISqlServerDatabase>();
            _loggerMock = new Mock<ILogger<CatalogRepository>>();
            _repository = CreateRepository(_databaseMock.Object, configuration, _loggerMock.Object);
        }

        private CatalogRepository CreateRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<CatalogRepository> logger)
        {
            CatalogRepository repository = new CatalogRepository(_databaseMock.Object, configuration, _loggerMock.Object);
            return repository;
        }

        /*
         *---------------------------------------------------------
         *---------------| GetMovementTypes use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetMovementTypes_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("MovementType", typeof(string));

            table.Rows.Add(1, "Entry");
            table.Rows.Add(2, "Exit");

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
            var result = await _repository.GetMovementTypes();

            // Assert
            result.ErrorCode.Should().Be(0);

            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.First().Id.Should().Be(1);
            result.Data.First().Type.Should().Be("Entry");
            result.Data.Last().Id.Should().Be(2);
            result.Data.Last().Type.Should().Be("Exit");
        }


        [Fact]
        public async Task GetMovementTypes_ShouldReturnError_WhenStoredProcedureFails()
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
            var result = await _repository.GetMovementTypes();

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
                .ThrowsAsync(new Exception($"Error executing getMovementTypes"));

            // Act
            Func<Task> act = async () =>
                await _repository.GetMovementTypes();

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing getMovementTypes");
        }

        /*
         *---------------------------------------------------------
         *---------------| Constructor tests |---------------------
         *---------------------------------------------------------
        */
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenDatabaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CatalogRepository(
                    null!,
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build(),
                    _loggerMock.Object
                )
            );
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CatalogRepository(
                    _databaseMock.Object,
                    null!,
                    _loggerMock.Object
                )
            );
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CatalogRepository(
                    _databaseMock.Object,
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(configurationData)
                        .Build(),
                    null!
                )
            );
        }
    }
}
