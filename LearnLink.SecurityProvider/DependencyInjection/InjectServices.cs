using LearnLink.Application.Security;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.SecurityProvider.DependencyInjection;

public static class InjectServices
{
    public static IServiceCollection AddEncryption(this IServiceCollection services)
    {
        services.AddSingleton<IEncryptionService, EncryptionService>();
        return services;
    }
}
