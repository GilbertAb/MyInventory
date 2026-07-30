using External.MyInventoryApi.Application.Contracts.DTOs.Request;
using External.MyInventoryApi.Application.Contracts.Messaging;
using External.MyInventoryApi.Application.Contracts.Results;
using External.MyInventoryApi.Application.Services;
using External.MyInventoryApi.Business.Entities;
using External.MyInventoryApi.Business.Messaging.Commands;
using External.MyInventoryApi.DataAccess.Contracts.InputModels;
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
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly MovementService _service;

        public MovementServiceTests ()
        {
            _repositoryMock = new Mock<IMovementRepository>();
            _eventBusMock = new Mock<IEventBus>();
            _service = new MovementService( _repositoryMock.Object, _eventBusMock.Object );
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

        /*
         *---------------------------------------------------------
         *---------------| RegisterMovement use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task RegisterMovement_ShouldReturnMappedResult_WhenRepositoryReturnsSuccess()
        {
            // Arrange
            var request = new RegisterMovementRequest
            {
                ProductId = 1,
                MovementTypeId = 1,
                Quantity = 10,
                MovementDescription = "IN"
            };

            _repositoryMock
                .Setup(r => r.RegisterProductMovement(It.IsAny<RegisterProductMovementInputModel>()))
                .ReturnsAsync(new OperationResult<int?>
                {
                    Data = 5,
                    ErrorCode = 0
                });

            // Act
            var result = await _service.RegisterMovement(request);

            // Assert
            result.ErrorCode.Should().Be(0);
            result.Data.Should().NotBeNull();
            result.Data!.Id.Should().Be(5);

            _repositoryMock.Verify(
                r => r.RegisterProductMovement(It.IsAny<RegisterProductMovementInputModel>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterMovement_ShouldReturnError_WhenRequestIsNull()
        {
            // Act
            var result = await _service.RegisterMovement(null!);

            // Assert
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Request can't be null");

            _repositoryMock.Verify(
                r => r.RegisterProductMovement(It.IsAny<RegisterProductMovementInputModel>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterMovement_ShouldReturnError_WhenRepositoryReturnsError()
        {
            // Arrange
            var request = new RegisterMovementRequest
            {
                ProductId = 1,
                MovementTypeId = 1,
                Quantity = 10,
                MovementDescription = "IN"
            };

            _repositoryMock
                .Setup(r => r.RegisterProductMovement(It.IsAny<RegisterProductMovementInputModel>()))
                .ReturnsAsync(new OperationResult<int?>
                {
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = "Database error"
                });

            // Act
            var result = await _service.RegisterMovement(request);

            // Assert
            result.ErrorCode.Should().Be(500);
            result.ErrorMessage.Should().Be("Database error");
        }

        /*
         *---------------------------------------------------------
         *---------------| GetProductStockHistory use case |-----------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task GetProductStockHistory_ShouldReturnMappedResult_WhenRepositoryReturnsData()
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
                    ProductId = 1,
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
                .Setup(r => r.GetProductStockHistory(12))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetProductStockHistory(12);


            // Assert

            result.Data.Should().NotBeNull();
            result.ErrorCode.Should().Be(0);

            result.Data.Should().BeEquivalentTo(movements, options =>
                options
                    .Excluding(x => x.CreatedAt)
            );

            _repositoryMock.Verify(r => r.GetProductStockHistory(12), Times.Once());
        }

        [Fact]
        public async Task GetProductStockHistory_ShouldReturnEmptyCollection_WhenNoMovementsExist()
        {
            // Arrange
            var operationResult = new OperationResult<IEnumerable<Movement>?>
            {
                Data = [],
                ErrorCode = 0
            };

            _repositoryMock
                .Setup(r => r.GetProductStockHistory(12))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _service.GetProductStockHistory(12);

            // Assert
            result.Data.Should().BeEmpty();
            result.ErrorCode.Should().Be(0);

            _repositoryMock.Verify(r => r.GetProductStockHistory(12), Times.Once());
        }

        [Fact]
        public async Task GetProductStockHistory_ShouldReturnError_WhenRepositoryReturnsError()
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

        /*
         *---------------------------------------------------------
         *---------------| PublishRegisterMovement use case |---------------
         *---------------------------------------------------------
        */
        [Fact]
        public async Task PublishRegisterMovement_ShouldReturnMappedResult_WhenRepositoryReturnsSuccess()
        {
            // Arrange
            var request = new RegisterMovementRequest
            {
                ProductId = 1,
                MovementTypeId = 1,
                Quantity = 10,
                MovementDescription = "IN"
            };

            _eventBusMock
                .Setup(x => x.PublishAsync(It.IsAny<RegisterMovementCommand>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await _service.PublishRegisterMovement(request);

            // Assert
            result.ErrorCode.Should().Be(0);

            _eventBusMock.Verify(
                x => x.PublishAsync(It.Is<RegisterMovementCommand>(c =>
                    c.ProductId == request.ProductId &&
                    c.MovementTypeId == request.MovementTypeId &&
                    c.Quantity == request.Quantity &&
                    c.MovementDescription == request.MovementDescription)
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task PublishRegisterMovement_ShouldReturnError_WhenRequestIsNull()
        {
            // Act
            var result = await _service.PublishRegisterMovement(null!);

            // Assert
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Request can't be null");

            _eventBusMock.Verify(
                x => x.PublishAsync(It.IsAny<RegisterMovementCommand>()),
                Times.Never
            );
        }

        [Fact]
        public async Task PublishRegisterMovement_ShouldReturnError_WhenMassTransitFails()
        {
            // Arrange
            var request = new RegisterMovementRequest
            {
                ProductId = 1,
                MovementTypeId = 1,
                Quantity = 10,
                MovementDescription = "IN"
            };

            _eventBusMock
                .Setup(x => x.PublishAsync(It.IsAny<RegisterMovementCommand>()))
                .ThrowsAsync(new Exception("RabbitMQ error"));

            // Act
            Func<Task> act = () => _service.PublishRegisterMovement(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("RabbitMQ error");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new MovementService(null!, _eventBusMock.Object)
            );
        }
    }
}
