using PandaRedu.SecurityProvider;

namespace PandaRedu.Api.Extensions;

/// <summary>
/// Helper extensions for <see cref="IConfiguration"/> to read specific
/// application configuration values.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets the launch URL from configuration.
    /// </summary>
    public static string Url(this IConfiguration config)
    {
        return config.GetSection("LaunchUrl").Get<string>()!;
    }

    /// <summary>
    /// Gets the client URL from configuration.
    /// </summary>
    public static string ClientUrl(this IConfiguration config)
    {
        return config.GetSection("ClientUrl").Get<string>()!;
    }

    /// <summary>
    /// Binds and returns <see cref="AuthenticationOptions"/> from configuration.
    /// </summary>
    public static AuthenticationOptions GetAuthenticationOptions(this IConfiguration config)
    {
        var authenticationOptions = config.GetSection("Authentication")?.Get<AuthenticationOptions>();
        ArgumentNullException.ThrowIfNull(authenticationOptions);

        return authenticationOptions;
    }
}
