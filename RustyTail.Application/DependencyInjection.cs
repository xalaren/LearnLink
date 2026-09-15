using RustyTail.Application.Security;
using RustyTail.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace RustyTail.Application;

/// <summary>
/// Registers application services
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds application services to the
        /// provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        public IServiceCollection AddApplication()
        {
            return services
                .AddUserServices()
                .AddAuthenticationServices();
        }
    }
}
