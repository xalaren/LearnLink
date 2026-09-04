using LearnLink.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;
using Microsoft.OpenApi;

namespace LearnLink.Api.Extensions;

internal static class ServiceCollectionExtensions
{
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
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });

                return Task.CompletedTask;
            });
        });

        return services;
    }

    internal static IServiceCollection AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider => configuration.GetAuthenticationOptions());
        return services;
    }
}