using LearnLink.Application.Messaging;
using LearnLink.Application.Users.CommandHandlers;
using LearnLink.Application.Users.Commands;
using LearnLink.Application.Users.Queries;
using LearnLink.Application.Users.QueryHandlers;
using LearnLink.Application.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application.Users;

internal static class DependencyInjection
{
    internal static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        return services
            .AddSeedingService()
            .AddCommandHandler<RegisterCommandHandler, RegisterCommand, RegisterCommandValidator>()
            .AddQueryHandler<ListQueryHandler, ListQuery, ListQueryResult, ListQueryValidator>();
    }

    private static IServiceCollection AddSeedingService(this IServiceCollection services)
    {
        return services.AddTransient<SeedingService>();
    }
}
