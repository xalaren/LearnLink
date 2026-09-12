using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Messaging.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Messaging;

internal static class DependencyInjection
{
    internal static IServiceCollection AddCommandHandler<TCommandHandler, TCommand, TValidator>(this IServiceCollection services)
        where TCommandHandler : class, ICommandHandler<TCommand>
        where TCommand : ICommand
        where TValidator : AbstractValidator<TCommand>
    {
        return services
            .AddScoped<TCommandHandler>()
            .AddScoped<TValidator>()
            .AddScoped<ICommandHandler<TCommand>>
            (
                provider => new LoggingCommandHandler<TCommand>
                (
                    provider.GetRequiredService<ILogger<TCommand>>(),
                    new ValidationCommandHandler<TCommand>
                    (
                        provider.GetRequiredService<TValidator>(),
                        provider.GetRequiredService<TCommandHandler>()
                    )
                )
            );
    }

    internal static IServiceCollection AddCommandHandler<TCommandHandler, TCommand, TResult, TValidator>(this IServiceCollection services)
        where TCommandHandler : class, ICommandHandler<TCommand, TResult>
        where TCommand : ICommand
        where TValidator : AbstractValidator<TCommand>

    {
        return services
            .AddScoped<TCommandHandler>()
            .AddScoped<TValidator>()
            .AddScoped<ICommandHandler<TCommand, TResult>>
            (
                provider => new LoggingCommandHandler<TCommand, TResult>
                (
                    provider.GetRequiredService<ILogger<TCommand>>(),
                    new ValidationCommandHandler<TCommand, TResult>
                    (
                        provider.GetRequiredService<TValidator>(),
                        provider.GetRequiredService<TCommandHandler>()
                    )
                )
            );
    }

    internal static IServiceCollection AddQueryHandler<TQueryHandler, TQuery, TResult, TValidator>(this IServiceCollection services)
        where TQueryHandler : class, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery
        where TValidator : AbstractValidator<TQuery>

    {
        return services
            .AddScoped<TQueryHandler>()
            .AddScoped<TValidator>()
            .AddScoped<IQueryHandler<TQuery, TResult>>
            (
                provider => new LoggingQueryHandler<TQuery, TResult>
                (
                    provider.GetRequiredService<ILogger<TQuery>>(),
                    new ValidationQueryHandler<TQuery, TResult>
                    (
                        provider.GetRequiredService<TValidator>(),
                        provider.GetRequiredService<TQueryHandler>()
                    )
                )
            );
    }
}
