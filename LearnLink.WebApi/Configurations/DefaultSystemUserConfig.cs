using LearnLink.Shared.Users;

namespace LearnLink.WebApi.Configurations
{
    public class DefaultSystemUserConfig
    {
        public DefaultSystemUser GetDefaultSystemUser(IConfiguration config) =>
             config.GetSection("DefaultSystemUser").Get<DefaultSystemUser>()!;
    }
}
