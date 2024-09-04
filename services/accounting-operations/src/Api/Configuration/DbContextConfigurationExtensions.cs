using AccountingOperations.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountingOperations.Api.Configuration;

public static class DbContextConfigurationExtensions
{
    public static IServiceCollection ConfigureDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        const string connectionKey = "Operations";

        var connectionString = configuration.GetConnectionString(connectionKey)
            ?? throw new InvalidOperationException($"Missing {connectionKey} connection string");

        return services.AddDbContext<OperationsDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
