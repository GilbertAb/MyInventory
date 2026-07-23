using External.MyInventoryApi.Application.Contracts.Messaging;
using MassTransit;

namespace External.MyInventoryApi.CrossCutting.Messaging
{
    public sealed class RabbitMqEventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqEventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T integrationEvent)
            where T : class
        {
            return _publishEndpoint.Publish(integrationEvent);
        }
    }
}
