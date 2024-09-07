namespace AccountingOperations.Core.Infrastructure.Broker;

public interface IMessageExchangeBus
{
    Task Send<TMessage>(TMessage message);
}
