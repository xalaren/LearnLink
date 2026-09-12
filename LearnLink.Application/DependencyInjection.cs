using LearnLink.Application.Security;
using LearnLink.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddUserServices()
            .AddAuthenticationServices();
    }
}
