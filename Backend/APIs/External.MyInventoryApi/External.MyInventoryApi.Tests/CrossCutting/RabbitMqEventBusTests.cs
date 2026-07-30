using External.MyInventoryApi.Business.Messaging.Commands;
using External.MyInventoryApi.CrossCutting.Messaging;
using MassTransit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Tests.CrossCutting
{
    public class RabbitMqEventBusTests
    {
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly RabbitMqEventBus _eventBus;

        public RabbitMqEventBusTests()
        {
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _eventBus = new RabbitMqEventBus(_publishEndpointMock.Object);
        }

        [Fact]
        public async Task PublishAsync_ShouldCallPublishEndpoint()
        {
            // Arrange
            var command = new RegisterMovementCommand
            {
                ProductId = 1,
                MovementTypeId = 1,
                Quantity = 10,
                MovementDescription = "IN"
            };

            _publishEndpointMock
                .Setup(x => x.Publish(
                    It.IsAny<RegisterMovementCommand>(),
                    It.IsAny<CancellationToken>())
                )
                .Returns(Task.CompletedTask);

            // Act
            await _eventBus.PublishAsync(command);

            // Assert
            _publishEndpointMock.Verify(
                x => x.Publish(
                    It.Is<RegisterMovementCommand>(c =>
                        c.ProductId == command.ProductId &&
                        c.MovementTypeId == command.MovementTypeId &&
                        c.Quantity == command.Quantity &&
                        c.MovementDescription == command.MovementDescription),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
