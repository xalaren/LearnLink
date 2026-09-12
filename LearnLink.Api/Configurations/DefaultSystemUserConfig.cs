using LearnLink.Application.Users.Models;

namespace LearnLink.Api.Configurations;
/// <summary>
/// Configuration for default system user
/// </summary>
public class DefaultSystemUserConfig
{
    /// <summary>
    /// Gets default system user from configuration
    /// </summary>
    /// <param name="config">Respective configuration</param>
    /// <returns>Default system user request</returns>
    public InitializeSystemUserRequest GetDefaultSystemUser(IConfiguration config) =>
         config.GetSection("DefaultSystemUser").Get<InitializeSystemUserRequest>()!;
}
