using LearnLink.Application.Security.Services;
using LearnLink.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddUsers();
        services.AddScoped<AuthenticationService>();

        return services;
    }
}
