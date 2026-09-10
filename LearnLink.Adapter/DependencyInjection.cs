using LearnLink.Adapter.Contexts;
using LearnLink.Application.Data;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Adapter;

public static class DependencyInjection 
{
    public static IServiceCollection AddAdapter(this IServiceCollection services)
    {
        return services.AddScoped<IApplicationDataContext, AppDbContext>();
    }
}
