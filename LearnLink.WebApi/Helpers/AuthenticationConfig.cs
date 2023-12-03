using LearnLink.SecurityProvider;

namespace LearnLink.WebApi.Helpers
{
    public static class AuthenticationConfig
    {
        public static AuthenticationOptions GetAuthenticationOptions(IConfiguration config) =>
            config.GetSection("AuthenticationOptions").Get<AuthenticationOptions>()!;
    }
}
