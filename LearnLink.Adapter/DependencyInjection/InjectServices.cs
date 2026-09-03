using System.Security.Cryptography.X509Certificates;
using LearnLink.Adapter.Repositories;
using LearnLink.Adapter.Transactions;
using LearnLink.Application.Repositories;
using LearnLink.Application.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Adapter.DependencyInjection;

public static class InjectServices 
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICredentialsRepository, CredentialsRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
