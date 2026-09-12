using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

namespace LearnLink.Api.Extensions;

/// <summary>
/// Service collection helper extensions used to configure OpenAPI and authentication options.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds OpenAPI (Swagger) configuration and configures a Bearer security scheme for JWT authentication.
    /// </summary>
    internal static IServiceCollection AddOpenApiWithAuth(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, ct) =>
            {
                document.Components ??= new OpenApiComponents();

                document.Components.SecuritySchemes?["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token",
                    Name = "Authorization",
                };

                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });

                return Task.CompletedTask;
            });
        });

        return services;
    }

    /// <summary>
    /// Registers <see cref="AuthenticationOptions"/> bound from configuration as a singleton.
    /// </summary>
    internal static IServiceCollection AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider => configuration.GetAuthenticationOptions());
        return services;
    }
}
