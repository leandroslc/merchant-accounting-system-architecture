using DailyBalances.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DailyBalances.Api.Configuration;

public static class DbContextConfigurationExtensions
{
    public static IServiceCollection ConfigureDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        const string connectionKey = "Balances";

        var connectionString = configuration.GetConnectionString(connectionKey)
            ?? throw new InvalidOperationException($"Missing {connectionKey} connection string");

        return services.ConfigureDbContext(connectionString);
    }

    public static IServiceCollection ConfigureDbContext(
        this IServiceCollection services,
        string connectionString)
    {
        return services.AddDbContext<BalancesDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
