using DailyBalances.Core.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DailyBalances.IntegrationTests.Fixtures;

public abstract class BaseFixture<TEntrypoint> : IDisposable, IAsyncLifetime
    where TEntrypoint : class
{
    public BaseFixture()
    {
        Factory = CreateFactory();

        DbContext = Factory.Services.GetRequiredService<BalancesDbContext>();
    }

    public WebApplicationFactory<TEntrypoint> Factory { get; }
    public BalancesDbContext DbContext { get; }

    public abstract WebApplicationFactory<TEntrypoint> CreateFactory();

    public void Dispose()
    {
        DbContext.Dispose();
        Factory.Dispose();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual async Task InitializeAsync()
    {
        await new MigrationsRunner(Factory.Services).RunMigrationsAsync();
    }
}
