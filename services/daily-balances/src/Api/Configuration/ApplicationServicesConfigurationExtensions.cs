using DailyBalances.Core;
using DailyBalances.Core.Infrastructure.Repositories;

namespace DailyBalances.Api.Configuration;

public static class ApplicationServicesConfigurationExtensions
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(
                typeof(Entrypoint).Assembly);
        });

        services.AddScoped<IBalanceRepository, BalanceRepository>();

        return services;
    }
}
