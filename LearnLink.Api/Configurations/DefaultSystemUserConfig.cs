using LearnLink.Shared.Model.Users;

namespace LearnLink.Api.Configurations
{
    public class DefaultSystemUserConfig
    {
        public DefaultSystemUser GetDefaultSystemUser(IConfiguration config) =>
             config.GetSection("DefaultSystemUser").Get<DefaultSystemUser>()!;
    }
}
