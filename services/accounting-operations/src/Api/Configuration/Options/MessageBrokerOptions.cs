using AccountingOperations.Api.Infrastructure;

namespace AccountingOperations.Api.Configuration.Options;

public sealed class MessageBrokerOptions
{
    public const string Section = "MessageBroker";

    public required Uri Host { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public required string OperationDoneTopic { get; init; }

    public required ExchangeTopics ExchangeTopics { get; init; }
}
