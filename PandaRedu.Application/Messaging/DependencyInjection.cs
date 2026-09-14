using FluentValidation;
using PandaRedu.Application.Abstractions.Messaging;
using PandaRedu.Application.Messaging.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PandaRedu.Application.Messaging;

/// <summary>
/// Provides helper methods to register messaging handlers and their
/// associated behaviours (validation, logging) into the dependency
/// injection container.
/// </summary>
internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers a command handler pipeline for <typeparamref name="TCommand"/>.
        /// The pipeline composes the concrete <typeparamref name="TCommandHandler"/>
        /// with validation and logging behaviours.
        /// </summary>
        /// <typeparam name="TCommandHandler">Concrete command handler type.</typeparam>
        /// <typeparam name="TCommand">Command type handled by the pipeline.</typeparam>
        /// <typeparam name="TValidator">FluentValidation validator for the command.</typeparam>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        internal IServiceCollection AddCommandHandler<TCommandHandler, TCommand, TValidator>()
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

        /// <summary>
        /// Registers a command handler pipeline for <typeparamref name="TCommand"/>
        /// that returns a result of type <typeparamref name="TResult"/>. The
        /// pipeline composes the concrete handler with validation and logging
        /// behaviours.
        /// </summary>
        /// <typeparam name="TCommandHandler">Concrete command handler type.</typeparam>
        /// <typeparam name="TCommand">Command type handled by the pipeline.</typeparam>
        /// <typeparam name="TResult">Result type returned by the handler.</typeparam>
        /// <typeparam name="TValidator">FluentValidation validator for the command.</typeparam>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        internal IServiceCollection AddCommandHandlerWithResult<TCommandHandler, TCommand, TResult, TValidator>()
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

        /// <summary>
        /// Registers a query handler pipeline for <typeparamref name="TQuery"/>
        /// that returns a <typeparamref name="TResult"/>. The pipeline composes
        /// the concrete query handler with validation and logging behaviours.
        /// </summary>
        /// <typeparam name="TQueryHandler">Concrete query handler type.</typeparam>
        /// <typeparam name="TQuery">Query type handled by the pipeline.</typeparam>
        /// <typeparam name="TResult">Result type returned by the query handler.</typeparam>
        /// <typeparam name="TValidator">FluentValidation validator for the query.</typeparam>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        internal IServiceCollection AddQueryHandler<TQueryHandler, TQuery, TResult, TValidator>()
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
}
