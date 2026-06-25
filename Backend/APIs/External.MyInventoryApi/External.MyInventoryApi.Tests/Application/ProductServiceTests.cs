using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Contracts.Services;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.DataAccess.Contracts.Repositories;
using External.MyInventoryApi.DataAccess.Contracts.Results;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.Application
{
    public class ProductServiceTests
    {
        //
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _service = new ProductService(_repositoryMock.Object);
        }

        // GetAllProducts use case
        [Fact]
        public async Task GetAllProducts_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ProductName = "Test",
                    Stock = 10,
                    Category = "Test"
                },
                new Product
                {
                    Id = 1,
                    ProductName = "Test",
                    Stock = 10,
                    Category = "Test"
                }
            };
            //OperationResult<IEnumerable<Product>?>
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = products,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(products, options =>
                options
                    .Excluding(x => x.CreatedAt)
                    .Excluding(x => x.UpdatedAt)
            );

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnEmptyCollection_WhenNoProductsExist()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = [],
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(0);
            result.Data.Should().BeEmpty();

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Product>?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.GetAllProducts())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetAllProducts();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.GetAllProducts(), Times.Once());

        }

    }
}
