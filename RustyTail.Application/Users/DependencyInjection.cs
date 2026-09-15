using RustyTail.Application.Messaging;
using RustyTail.Application.Users.CommandHandlers;
using RustyTail.Application.Users.Commands;
using RustyTail.Application.Users.Queries;
using RustyTail.Application.Users.QueryHandlers;
using RustyTail.Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace RustyTail.Application.Users;

/// <summary>
/// Registers users-related application services
/// </summary>
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds users services (register, list handlers, seeding, etc.) to the
        /// provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        internal IServiceCollection AddUserServices()
        {
            return services
                .AddSeedingService()
                .AddCommandHandler<RegisterCommandHandler, RegisterCommand, RegisterCommandValidator>()
                .AddQueryHandler<ListQueryHandler, ListQuery, ListQueryResult, ListQueryValidator>();
        }

        private IServiceCollection AddSeedingService()
        {
            return services.AddTransient<SeedingService>();
        }
    }
}
