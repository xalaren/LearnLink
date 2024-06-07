namespace LearnLink.WebApi.Helpers
{
    public class RegisterConfig
    {
        public AdminDefaultAuthenticationData GetAdminDefaultAuthenticationData(IConfiguration config) =>
             config.GetSection("AdminDefaultAuthenticationData").Get<AdminDefaultAuthenticationData>()!;
    }
}
