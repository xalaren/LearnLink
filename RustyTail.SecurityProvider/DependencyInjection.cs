using RustyTail.Application.Security.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace RustyTail.SecurityProvider;

/// <summary>
/// Registers security provider implementations into the application's
/// dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the security provider services (encryption and token provider)
    /// to the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to register services with.</param>
    public static void AddSecurityProvider(this IServiceCollection services)
    {
        services.AddSingleton<IEncryptionProvider, EncryptionProvider>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
    }
}
