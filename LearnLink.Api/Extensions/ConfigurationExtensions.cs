namespace LearnLink.Api.Extensions
{
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
    }
}
