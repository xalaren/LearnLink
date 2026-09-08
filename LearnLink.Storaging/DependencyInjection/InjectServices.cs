using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Storaging.DependencyInjection;

public static class InjectServices
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string rootDirectory)
    {
        services.AddSingleton(provider => Storage.Instance(rootDirectory));

        return services;
    }
}
