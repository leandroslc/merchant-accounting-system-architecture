using DailyBalances.Api.Configuration;
using DailyBalances.Core.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DailyBalances.IntegrationTests.Fixtures;

public class ApiFixture : IDisposable, IAsyncLifetime
{
    public ApiFixture()
    {
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder
                .UseEnvironment("Test")
                .ConfigureServices((context, services) => services
                    .ConfigureMigrations(context.Configuration))
            );

        DbContext = Factory.Services.GetRequiredService<BalancesDbContext>();
    }

    public WebApplicationFactory<Program> Factory { get; }
    public BalancesDbContext DbContext { get; }
    public HttpClient Client { get; private set; } = null!;

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
    }
}
