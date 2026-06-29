using External.MyInventoryApi.Application.Contracts.DTOs;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.Business.Catalogs;
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
    public class CatalogServiceTests
    {
        private readonly Mock<ICatalogRepository> _repositoryMock;
        private readonly CatalogService _service;

        public CatalogServiceTests()
        {
            _repositoryMock = new Mock<ICatalogRepository>();
            _service = new CatalogService(_repositoryMock.Object);
        }

        /*
         *---------------------------------------------------------
         *---------------| GetMovementTypes use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetMovementTypes_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var movementTypes = new List<MovementType>
            {
                new MovementType
                {
                    Id = 1,
                    Type = "Entry"
                },
                new MovementType
                {
                    Id = 1,
                    Type = "Exit"
                }
            };

            var operationResult = new OperationResult<IEnumerable<MovementType>?>
            {
                Data = movementTypes,
                ErrorCode = 0,
                ErrorMessage = "Succeed"
            };

            _repositoryMock
                .Setup(r => r.GetMovementTypes())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetMovementTypes();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(movementTypes);

            _repositoryMock.Verify(r => r.GetMovementTypes(), Times.Once());
        }

        [Fact]
        public async Task GetMovementTypes_ShouldReturnEmptyCollection_WhenNoProductsExist()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<MovementType>?>
            {
                Data = [],
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetMovementTypes())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetMovementTypes();

            // Assert
            result.ErrorCode.Should().Be(0);
            result.Data.Should().BeEmpty();

            _repositoryMock.Verify(r => r.GetMovementTypes(), Times.Once());
        }

        [Fact]
        public async Task GetMovementTypes_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<MovementType>?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.GetMovementTypes())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetMovementTypes();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.GetMovementTypes(), Times.Once());

        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CatalogService(null!)
            );
        }
    }
}
