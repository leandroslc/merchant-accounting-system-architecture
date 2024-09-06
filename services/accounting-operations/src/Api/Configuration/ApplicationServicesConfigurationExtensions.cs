using AccountingOperations.Core;
using AccountingOperations.Core.Infrastructure.Repositories;

namespace AccountingOperations.Api.Configuration;

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

        services.AddScoped<IAccountingOperationRepository, AccountingOperationRepository>();

        return services;
    }
}
