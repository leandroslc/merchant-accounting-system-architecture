using AccountingOperations.Core;
using FluentMigrator.Runner;

namespace AccountingOperations.Api.Configuration;

public static class MigrationConfigurationExtensions
{
    public static IServiceCollection ConfigureMigrations(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

        return services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Entrypoint).Assembly));
    }
}
