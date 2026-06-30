using External.MyInventoryApi.Business.Entities;
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
    public class ProductRepositoryTests
    {
        private readonly Mock<ISqlServerDatabase> _databaseMock;
        private readonly Mock<ILogger<ProductRepository>> _loggerMock;
        private readonly ProductRepository _repository;
        Dictionary<string, string?> configurationData = new Dictionary<string, string?>
        {
            ["StoredProcedures:SP_ADD_PRODUCT"] = "MyInventory.usp_AddProduct",
            ["StoredProcedures:SP_DELETE_PRODUCT"] = "MyInventory.usp_DeleteProduct",
            ["StoredProcedures:SP_GET_PRODUCTS"] = "MyInventory.usp_GetProducts",
            ["StoredProcedures:SP_GET_PRODUCT_BY_ID"] = "MyInventory.usp_GetProductById",
            ["StoredProcedures:SP_UPDATE_PRODUCT"] = "MyInventory.usp_UpdateProduct",
            ["StoredProcedures:SP_GET_PRODUCT_STOCK_SUMMARY"] = "MyInventory.usp_GetProductStockSummary"
        };

        /*
         *---------------------------------------------------------
         *---------------| Constructor tests |---------------------
         *---------------------------------------------------------
        */
        public ProductRepositoryTests ()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();
            
            _databaseMock = new Mock<ISqlServerDatabase>();
            _loggerMock = new Mock<ILogger<ProductRepository>>();
            _repository = CreateRepository(_databaseMock.Object, configuration, _loggerMock.Object);//new ProductRepository(_databaseMock.Object, configuration, _loggerMock.Object);
        }

        private ProductRepository CreateRepository(ISqlServerDatabase database, IConfiguration configuration,
            ILogger<ProductRepository> logger)
        {
            ProductRepository repository = new ProductRepository(_databaseMock.Object, configuration, _loggerMock.Object);
            return repository;
        }

        /*
         *---------------------------------------------------------
         *---------------| AddProduct use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task AddProduct_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Func<Task> act = async () => await _repository.AddProduct(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddProduct_ShouldCallStoredProcedureWithCorrectParameters()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Test",
                Category = "Test",
                Stock = 15
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
            await _repository.AddProduct(product);

            // Assert
            _databaseMock.Verify(
                d => d.ExecuteAsync(
                    "MyInventory.usp_AddProduct",
                    It.Is<Dictionary<string, object>>(p =>
                        (string)p["@ProductName"] == "Test" &&
                        (string)p["@Category"] == "Test" &&
                        (int)p["@Stock"] == 15)
                ),
                Times.Once);
        }

        [Fact]
        public async Task AddProduct_ShouldThrowArgumentException_WhenProductNameIsEmpty()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "",
                Category = "Test",
                Stock = 10
            };

            // Act
            Func<Task> act = () => _repository.AddProduct(product);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddProduct_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Test",
                Category = "Test",
                Stock = 15
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
            var result = await _repository.AddProduct(product);

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenStoredProcedureFails()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Test",
                Category = "Test",
                Stock = 15
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
            var result = await _repository.AddProduct(product);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task AddProduct_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Test",
                Category = "Test",
                Stock = 15
            };

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing add product: {product.ProductName}"));

            // Act
            Func<Task> act = async () =>
                await _repository.AddProduct(product);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing add product: {product.ProductName}");
        }

        /*
         *---------------------------------------------------------
         *---------------| DeleteProduct use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task DeleteProduct_ShouldCallStoredProcedureWithCorrectParameters()
        {
            // Arrange
            var productId = 12;

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
            await _repository.DeleteProduct(productId);

            // Assert
            _databaseMock.Verify(
                d => d.ExecuteAsync(
                    "MyInventory.usp_DeleteProduct",
                    It.Is<Dictionary<string, object>>(p =>
                        (int)p["@ProductId"] == 12
                    )
                ),
                Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var productId = 12;

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
            var result = await _repository.DeleteProduct(productId);

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnError_WhenStoredProcedureFails()
        {
            // Arrange
            var productId = 12;

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
            var result = await _repository.DeleteProduct(productId);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteProduct_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            var productId = 12;

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing delete product: {productId}"));

            // Act
            Func<Task> act = async () =>
                await _repository.DeleteProduct(productId);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing delete product: {productId}");
        }

        /*
         *---------------------------------------------------------
         *---------------| GetAllProducts use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetAllProducts_ShouldReturnSuccessResult_WhenStoredProcedureSucceeds()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("CreatedAt", typeof(DateTime));
            table.Columns.Add("UpdatedAt", typeof(DateTime));

            table.Rows.Add(1, "Test", "Test", 10, DateTime.Now, null);
            table.Rows.Add(2, "Test 2", "Test 2", 10, DateTime.Now, null);

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
            var result = await _repository.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(0);

            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);

            result.Data!.First().Id.Should().Be(1);
            result.Data.First().ProductName.Should().Be("Test");
        }


        [Fact]
        public async Task GetAllProducts_ShouldReturnError_WhenStoredProcedureFails()
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
            var result = await _repository.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetAllProducts_ShouldThrowException_WhenStoredProcedureExecutionFails()
        {
            // Arrange
            var productId = 12;

            _databaseMock
                .Setup(d => d.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .ThrowsAsync(new Exception($"Error executing get products"));

            // Act
            Func<Task> act = async () =>
                await _repository.GetAllProducts();

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage($"Error executing get products");
        }
    }
}
