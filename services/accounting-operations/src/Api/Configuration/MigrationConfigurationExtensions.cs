using AccountingOperations.Core;
using FluentMigrator.Runner;

namespace AccountingOperations.Api.Configuration;

public static class MigrationConfigurationExtensions
{
    public static IServiceCollection ConfigureMigrations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        const string connectionKey = "Operations";

        var connectionString = configuration.GetConnectionString(connectionKey)
            ?? throw new InvalidOperationException($"Missing {connectionKey} connection string");

        return services.ConfigureMigrations(connectionString);
    }

    public static IServiceCollection ConfigureMigrations(
        this IServiceCollection services,
        string connectionString)
    {
        return services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Entrypoint).Assembly));
    }
}
