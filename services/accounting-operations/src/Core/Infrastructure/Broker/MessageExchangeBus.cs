using MassTransit;

namespace AccountingOperations.Core.Infrastructure.Broker;

public sealed class MessageExchangeBus : IMessageExchangeBus
{
    private readonly IPublishEndpoint publisher;

    public MessageExchangeBus(IPublishEndpoint publisher)
    {
        this.publisher = publisher;
    }

    public async Task Send<TMessage>(TMessage message)
    {
        await publisher.Publish(message ?? throw new ArgumentNullException(nameof(message)));
    }
}
