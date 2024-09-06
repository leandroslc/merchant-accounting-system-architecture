using AccountingOperations.Core;

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

        return services;
    }
}
