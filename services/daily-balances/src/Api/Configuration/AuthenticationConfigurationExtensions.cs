using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DailyBalances.Api.Configuration;

public static class AuthenticationConfigurationExtensions
{
    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    RequireSignedTokens = false,
                    RequireExpirationTime = false,
                    RequireAudience = false,
                    SignatureValidator = (token, _) => new JsonWebToken(token),
                };
            });

        return services;
    }
}
