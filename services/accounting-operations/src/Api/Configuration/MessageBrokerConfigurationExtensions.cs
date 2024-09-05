using AccountingOperations.Api.Configuration.Options;
using AccountingOperations.Api.Infrastructure;
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

        services.AddSingleton(new ExchangeTopics(messageBrokerOptions));
        services.AddSingleton<MessageExchangeBus>();

        return services.AddMassTransit(options =>
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
    }
}
