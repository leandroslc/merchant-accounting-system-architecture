using MassTransit;

namespace AccountingOperations.Core.Infrastructure.Broker;

public sealed class MessageExchangeBus
{
    private readonly IBus bus;

    public MessageExchangeBus(IBus bus)
    {
        this.bus = bus;
    }

    public async Task Send<TMessage>(TMessage message, string topic)
    {
        var endpoint = await bus.GetSendEndpoint(new Uri(topic));

        await endpoint.Send(message ?? throw new ArgumentNullException(nameof(message)));
    }
}
