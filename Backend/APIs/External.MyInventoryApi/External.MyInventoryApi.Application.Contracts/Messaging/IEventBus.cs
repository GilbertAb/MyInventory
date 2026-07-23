namespace External.MyInventoryApi.Application.Contracts.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T integrationEvent) where T : class;
    }
}
