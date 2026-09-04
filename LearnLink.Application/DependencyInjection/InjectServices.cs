using LearnLink.Application.Services;
using LearnLink.Application.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application.DependencyInjection;

public static class InjectServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<SeedingService>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, string rootDirectory)
    {
        services.AddSingleton(provider => Storage.Instance(rootDirectory));

        return services;
    }
}
