using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using DailyBalances.Api.Configuration;
using DailyBalances.Core.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DailyBalances.Cli.Commands;

[Description("Runs migrations")]
internal sealed class MigrationCommand : Command<MigrationCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("Connection string to connect to the database")]
        [CommandOption("-c|--connectionString")]
        public string? ConnectionString { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.ConnectionString is null)
        {
            AnsiConsole.MarkupLine("[red]Required parameter [blue]\"connectionString\"[/] not specified.[/]");

            return 1;
        }

        var services = new ServiceCollection()
            .ConfigureMigrations(settings.ConnectionString)
            .ConfigureDbContext(settings.ConnectionString);

        using var serviceProvider = services.BuildServiceProvider(validateScopes: false);

        var runner = new MigrationsRunner(serviceProvider);

        runner.RunMigrationsAsync().Wait();

        AnsiConsole.MarkupLine("[blue]Migrations applied successfully[/]");

        return 0;
    }
}
