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
    public class MovementServiceTests
    {
        private readonly Mock<IMovementRepository> _repositoryMock;
        private readonly MovementService _service;

        public MovementServiceTests ()
        {
            _repositoryMock = new Mock<IMovementRepository>();
            _service = new MovementService( _repositoryMock.Object );
        }


        /*
         *---------------------------------------------------------
         *---------------| GetMovements use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetMovements_ShouldReturnMappedResult_WhenRepositoryReturnsData()
        {
            // Arrange
            var movements = new List<Movement>
            {
                new Movement
                {
                    ProductId = 1,
                    MovementTypeId = 1,
                    MovementDate = DateTime.Now,
                    Quantity = 5,
                    MovementDescription = "movement"
                },
                new Movement
                {
                    ProductId = 2,
                    MovementTypeId = 2,
                    MovementDate = DateTime.Now,
                    Quantity = 15,
                    MovementDescription = "IN"
                }
            };

            var operationResult = new OperationResult<IEnumerable<Movement>?>
            {
                Data = movements,
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };


            _repositoryMock
                .Setup(r => r.GetMovements())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetMovements();


            // Assert

            result.Data.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);

            result.Data.Should().BeEquivalentTo(movements, options =>
                options
                    .Excluding(x => x.CreatedAt)
            );

            _repositoryMock.Verify(r => r.GetMovements(), Times.Once());
        }

        [Fact]
        public async Task GetAllMovements_ShouldReturnEmptyCollection_WhenNoMovementsExist()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Movement>?>
            {
                Data = [],
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetMovements())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetMovements();

            // Assert
            result.Data.Should().BeEmpty();
            result.ErrorCode.Should().Be(0);

            _repositoryMock.Verify(r => r.GetMovements(), Times.Once());
        }

        [Fact]
        public async Task GetAllMovements_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Movement>?>
            {
                Data = null,
                ErrorCode = 500,
                ErrorMessage = "Database error"
            };

            _repositoryMock
                .Setup(r => r.GetMovements())
                .ReturnsAsync(operationResult);


            // Act
            var result = await _service.GetMovements();

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");

            _repositoryMock.Verify(r => r.GetMovements(), Times.Once());
        }
    }
}
