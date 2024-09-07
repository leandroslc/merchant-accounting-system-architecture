using DailyBalances.Cli.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config
        .AddCommand<MigrationCommand>("migrate")
        .WithAlias("migration")
        .WithExample("migrate --connectionString \"Server=127.0.0.1;Database=test;\"")
        .WithExample("migrate --dev");
});

app.Run(args);
