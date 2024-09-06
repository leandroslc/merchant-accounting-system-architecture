using AccountingOperations.Api.Configuration.Options;
using AccountingOperations.Core.Infrastructure.Broker;
using MassTransit;

namespace AccountingOperations.Api.Configuration;

public static class MessageBrokerConfigurationExtensions
{
    public static IServiceCollection ConfigureMessageBrokerIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageBrokerOptions = configuration
            .GetRequiredSection(MessageBrokerOptions.Section)
            .Get<MessageBrokerOptions>()
            ?? throw new InvalidOperationException($"No \"{MessageBrokerOptions.Section}\" options provided");

        services.AddSingleton(messageBrokerOptions.ExchangeTopics);
        services.AddSingleton<MessageExchangeBus>();

        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, factory) =>
            {
                factory.Host(messageBrokerOptions.Host, host =>
                {
                    host.Username(messageBrokerOptions.Username);
                    host.Password(messageBrokerOptions.Password);
                });
                factory.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
