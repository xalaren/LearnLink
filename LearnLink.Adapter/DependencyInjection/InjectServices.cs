using LearnLink.Adapter.Contexts;
using LearnLink.Adapter.Transactions;
using LearnLink.Application.Repositories;
using LearnLink.Application.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Adapter.DependencyInjection;

public static class InjectServices 
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        return services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
