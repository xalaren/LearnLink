using LearnLink.SecurityProvider;

namespace LearnLink.WebApi.Configurations
{
    public class AuthenticationConfig
    {
        public AuthenticationOptions GetAuthenticationOptions(IConfiguration config) =>
            config.GetSection("AuthenticationOptions").Get<AuthenticationOptions>()!;
    }
}
