using AccountingOperations.Core;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace AccountingOperations.Api.Configuration;

public static class ValidatorsConfigurationExtensions
{
    public static IServiceCollection ConfigureValidations(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(Entrypoint).Assembly,
            ServiceLifetime.Singleton);

        services.AddFluentValidationAutoValidation(options
            => options.DisableBuiltInModelValidation = true);

        return services;
    }
}
