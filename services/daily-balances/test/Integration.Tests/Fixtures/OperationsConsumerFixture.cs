extern alias OperationsConsumer;

using DailyBalances.Api.Configuration;
using DailyBalances.OperationsConsumer.Configuration;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OperationsConsumerProgram = OperationsConsumer::Program;

namespace DailyBalances.IntegrationTests.Fixtures;

public class OperationsConsumerFixture : BaseFixture<OperationsConsumerProgram>
{
    public override WebApplicationFactory<OperationsConsumerProgram> CreateFactory()
    {
        return new WebApplicationFactory<OperationsConsumerProgram>()
            .WithWebHostBuilder(builder => builder
                .UseEnvironment("Test")
                .Configure((c) => {})
                .ConfigureServices((context, services) => services
                    .ConfigureMessageBrokerTestsIntegration(context.Configuration)
                    .ConfigureMigrations(context.Configuration)));
    }

    public ITestHarness MessageExchangeTestHarness { get; private set; } = null!;

    public override Task DisposeAsync()
    {
        return base.DisposeAsync();
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        MessageExchangeTestHarness = Factory.Services.GetTestHarness();
    }
}
