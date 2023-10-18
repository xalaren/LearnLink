using CoursesPrototype.SecurityProvider;

namespace CoursesPrototype.WebApi.Helpers
{
    public static class AuthenticationConfig
    {
        public static AuthenticationOptions GetAuthenticationOptions(IConfiguration config) =>
            config.GetSection("AuthenticationOptions").Get<AuthenticationOptions>()!;
    }
}
