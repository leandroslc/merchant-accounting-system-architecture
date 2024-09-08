using DailyBalances.OperationsConsumer.Configuration.Options;
using DailyBalances.OperationsConsumer.Consumer;
using MassTransit;

namespace DailyBalances.OperationsConsumer.Configuration;

public static class MessageBrokerConfigurationExtensions
{
    public static IServiceCollection ConfigureMessageBrokerIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageBrokerOptions = GetOptions(configuration);

        services.AddMassTransit(options =>
            options.ConfigureMassTransit(messageBrokerOptions));

        return services;
    }

    public static IServiceCollection ConfigureMessageBrokerTestsIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messageBrokerOptions = GetOptions(configuration);

        return services.AddMassTransitTestHarness(options =>
            options.ConfigureMassTransit(messageBrokerOptions));
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
        options.AddConsumer<OperationRegisteredConsumer>();

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
