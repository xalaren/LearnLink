using LearnLink.Application.Security.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.SecurityProvider.DependencyInjection;

public static class InjectServices
{
    public static void AddEncryption(this IServiceCollection services)
    {
        services.AddSingleton<IEncryptionProvider, EncryptionProvider>();
    }

    public static void AddTokenProvider(this IServiceCollection services)
    {
        services.AddSingleton<ITokenProvider, TokenProvider>();
    }
}
