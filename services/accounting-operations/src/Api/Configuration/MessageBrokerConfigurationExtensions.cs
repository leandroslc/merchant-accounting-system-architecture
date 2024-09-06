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
        var messageBrokerOptions = GetOptions(configuration);

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

    public static void ConfigureMassTransit(
        this IBusRegistrationConfigurator options,
        IConfiguration configuration)
    {
        var messageBrokerOptions = GetOptions(configuration);

        options.UsingRabbitMq((context, factory) =>
        {
            factory.Host(messageBrokerOptions.Host, host =>
            {
                host.Username(messageBrokerOptions.Username);
                host.Password(messageBrokerOptions.Password);
            });
            factory.ConfigureEndpoints(context);
        });
    }

    private static MessageBrokerOptions GetOptions(IConfiguration configuration)
    {
        return configuration
            .GetRequiredSection(MessageBrokerOptions.Section)
            .Get<MessageBrokerOptions>()
            ?? throw new InvalidOperationException($"No \"{MessageBrokerOptions.Section}\" options provided");
    }

    private static void ConfigureMassTransit(
        this IBusRegistrationConfigurator options,
        MessageBrokerOptions messageBrokerOptions)
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
    }
}
