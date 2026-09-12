using LearnLink.Application.Messaging;
using LearnLink.Application.Security.CommandHandlers;
using LearnLink.Application.Security.Commands;
using LearnLink.Application.Security.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application.Security;

/// <summary>
/// Registers security-related application services such as command
/// handlers for authentication into the dependency injection container.
/// </summary>
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds authentication services (login and refresh handlers) to the
        /// provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        internal IServiceCollection AddAuthenticationServices()
        {
            return services
                .AddCommandHandlerWithResult<LoginCommandHandler, LoginCommand, TokenPair, LoginCommandValidator>()
                .AddCommandHandlerWithResult<RefreshCommandHandler, RefreshCommand, TokenPair, RefreshCommandValidator>();
        }
    }
}
