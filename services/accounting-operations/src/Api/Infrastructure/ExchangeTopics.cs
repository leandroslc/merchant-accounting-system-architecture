using AccountingOperations.Api.Configuration.Options;

namespace AccountingOperations.Api.Infrastructure;

public sealed class ExchangeTopics
{
    public ExchangeTopics(MessageBrokerOptions options)
    {
        OperationDone = $"{options.Host}/{options.OperationDoneTopic}";
    }

    public string OperationDone { get; }
}
