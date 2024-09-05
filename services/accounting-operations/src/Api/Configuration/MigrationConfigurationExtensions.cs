using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace AccoutingOperations.Database.Configuration;

public static class MigrationConfigurationExtensions
{
    public static IServiceCollection ConfigureMigrations(
        this IServiceCollection services,
        string connectionString)
    {
        if (connectionString is null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationConfigurationExtensions).Assembly));
    }
}
