using LearnLink.SecurityProvider;

namespace LearnLink.Api.Configurations
{
    public class AuthenticationConfig
    {
        public AuthenticationOptions GetAuthenticationOptions(IConfiguration config) =>
            config.GetSection("AuthenticationOptions").Get<AuthenticationOptions>()!;
    }
}
