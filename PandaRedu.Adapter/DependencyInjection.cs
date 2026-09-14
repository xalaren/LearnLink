using PandaRedu.Adapter.Contexts;
using PandaRedu.Application.Abstractions.Data;
using Microsoft.Extensions.DependencyInjection;

namespace PandaRedu.Adapter;


/// <summary>
/// Provides extension methods to register adapter-layer services into the
/// application's dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the adapter implementations used by the application.
    /// Currently this method registers <see cref="IApplicationDataContext"/>
    /// with the concrete <see cref="AppDbContext"/> as a scoped service.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAdapter(this IServiceCollection services)
    {
        return services.AddScoped<IApplicationDataContext, AppDbContext>();
    }
}
