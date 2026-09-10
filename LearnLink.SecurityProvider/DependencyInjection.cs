using LearnLink.Application.Security.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.SecurityProvider;

public static class DependencyInjection
{
    public static void AddSecurityProvider(this IServiceCollection services)
    {
        services.AddSingleton<IEncryptionProvider, EncryptionProvider>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
    }
}
