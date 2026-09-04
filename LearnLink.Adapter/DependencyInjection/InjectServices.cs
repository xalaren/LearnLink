using LearnLink.Adapter.Contexts;
using LearnLink.Application.Data;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Adapter.DependencyInjection;

public static class InjectServices 
{
    public static IServiceCollection AddApplicationDataContext(this IServiceCollection services)
    {
        return services.AddScoped<IApplicationDataContext, AppDbContext>();
    }
}
