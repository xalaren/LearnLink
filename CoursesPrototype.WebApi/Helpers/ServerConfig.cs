namespace CoursesPrototype.WebApi.Helpers
{
    public static class ServerConfig
    {
        public static string Url(IConfiguration config)
        {
            return config.GetSection("LaunchUrl").Get<string>()!;
        }
    }
}
