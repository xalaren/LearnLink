using LearnLink.Application.Security.Services;
using LearnLink.Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<SeedingService>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();

        return services;
    }
}
