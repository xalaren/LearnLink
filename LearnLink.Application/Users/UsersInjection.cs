using LearnLink.Application.Messaging.Abstractions;
using LearnLink.Application.Messaging.CommandHandlersBehaviours;
using LearnLink.Application.Users.CommandHandlers;
using LearnLink.Application.Users.Commands;
using LearnLink.Application.Users.Services;
using LearnLink.Application.Users.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Users;

internal static class UsersInjection
{
    internal static IServiceCollection AddUsers(this IServiceCollection services)
    {
        return services
            .AddSeedingService()
            .AddRegisterHandlers();
    }

    private static IServiceCollection AddRegisterHandlers(this IServiceCollection services)
    {
        return services
            .AddScoped<RegisterCommandHandler>()
            .AddScoped<ICommandHandler<RegisterCommand>>
            (
                provider => new LoggingQueryHandler<RegisterCommand>
                (
                    provider.GetRequiredService<ILogger<RegisterCommand>>(),
                    new ValidationCommandHandler<RegisterCommand>
                    (
                        new RegisterCommandValidator(),
                        provider.GetRequiredService<RegisterCommandHandler>()
                    )
                )
            );
    }

    private static IServiceCollection AddSeedingService(this IServiceCollection services)
    {
        return services.AddTransient<SeedingService>();
    }
}
