using FluentMigrator.Runner;

namespace AccountingOperations.Api.Infrastructure;

public sealed class MigrationsRunner
{
    private readonly IServiceProvider serviceProvider;

    public MigrationsRunner(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task RunMigrationsAsync()
    {
        using var scope = serviceProvider.CreateScope();

        var archivesContext = scope.ServiceProvider.GetRequiredService<OperationsDbContext>();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        await archivesContext.Database.EnsureCreatedAsync();

        runner.MigrateUp();
    }
}
