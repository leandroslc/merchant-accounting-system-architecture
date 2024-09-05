namespace AccountingOperations.Api.Configuration;

public static class ApplicationServicesConfigurationExtensions
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(
                typeof(ApplicationServicesConfigurationExtensions).Assembly);
        });

        return services;
    }
}
