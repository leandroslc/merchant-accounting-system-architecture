using AccountingOperations.Api.Configuration;
using AccountingOperations.Core.Infrastructure.Data;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingOperations.IntegrationTests.Fixtures;

public class ApiFixture : IDisposable, IAsyncLifetime
{
    public ApiFixture()
    {
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .UseEnvironment("Test")
                .ConfigureServices((context, services) => services
                    .ConfigureMessageBrokerTestsIntegration(context.Configuration)
                    .ConfigureMigrations(context.Configuration))
            );

        DbContext = Factory.Services.GetRequiredService<OperationsDbContext>();
    }

    public WebApplicationFactory<Program> Factory { get; }
    public OperationsDbContext DbContext { get; }
    public HttpClient Client { get; private set; } = null!;
    public ITestHarness MessageExchangeTestHarness { get; private set; } = null!;

    public void Dispose()
    {
        DbContext.Dispose();
        Factory.Dispose();
    }

    public Task DisposeAsync()
    {
        Client.Dispose();

        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        await new MigrationsRunner(Factory.Services).RunMigrationsAsync();

        Client = Factory.CreateClient();
        MessageExchangeTestHarness = Factory.Services.GetTestHarness();
    }
}
