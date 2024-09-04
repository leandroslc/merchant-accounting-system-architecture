using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AccountingOperations.Api.Configuration;

public static class SwaggerConfigurationExtensions
{
    private const string AuthSchema = "Bearer";

    public static IServiceCollection ConfigureSwagger(
        this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(AuthSchema, new()
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = AuthSchema,
                Name = "Authorization",
                BearerFormat = "JWT",
            });

            options.OperationFilter<AuthorizationHeaderOperationFilter>();
        });
    }

    private sealed class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var hasAuthorizeFilter = metadata.Any(item => item is AuthorizeAttribute);
            var hasAnonymousFilter = metadata.Any(item => item is AllowAnonymousAttribute);

            if (hasAuthorizeFilter is false || hasAnonymousFilter)
            {
                return;
            }

            operation.Security ??= [];

            operation.Security.Add(new()
            {
                [
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = AuthSchema,
                            Type = ReferenceType.SecurityScheme,
                        },
                    }
                ] = Array.Empty<string>(),
            });
        }
    }
}
