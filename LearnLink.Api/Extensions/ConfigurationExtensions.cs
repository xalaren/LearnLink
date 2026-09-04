using LearnLink.SecurityProvider;

namespace LearnLink.Api.Extensions;

public static class ConfigurationExtensions
{
    public static string Url(this IConfiguration config)
    {
        return config.GetSection("LaunchUrl").Get<string>()!;
    }

    public static string ClientUrl(this IConfiguration config)
    {
        return config.GetSection("ClientUrl").Get<string>()!;
    }

    public static AuthenticationOptions GetAuthenticationOptions(this IConfiguration config)
    {
        var authenticationOptions = config.GetSection("Authentication")?.Get<AuthenticationOptions>();
        ArgumentNullException.ThrowIfNull(authenticationOptions);
        
        return authenticationOptions;
    }
}