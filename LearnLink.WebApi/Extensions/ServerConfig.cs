using LearnLink.WebApi.Helpers;

namespace LearnLink.WebApi.Extensions
{
    public static class ServerConfig
    {
        public static string Url(this IConfiguration config)
        {
            return config.GetSection("LaunchUrl").Get<string>()!;
        }

        public static string ClientUrl(this IConfiguration config)
        {
            return config.GetSection("ClientUrl").Get<string>()!;
        }

        public static AdminDefaultAuthenticationData AdminDefaultAuthenticationData(this IConfiguration config)
        {
            return config.GetSection("AdminDefaultAuthenticationData").Get<AdminDefaultAuthenticationData>()!;
        }
    }
}
