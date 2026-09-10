using LearnLink.Application.Users.Models;

namespace LearnLink.Api.Configurations;

public class DefaultSystemUserConfig
{
    public InitializeSystemUserRequest GetDefaultSystemUser(IConfiguration config) =>
         config.GetSection("DefaultSystemUser").Get<InitializeSystemUserRequest>()!;
}
