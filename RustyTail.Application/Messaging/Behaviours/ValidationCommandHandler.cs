using FluentValidation;
using RustyTail.Application.Abstractions.Messaging;
using RustyTail.Application.Shared.Responses;
using RustyTail.Application.Shared.Responses.Extensions;

namespace RustyTail.Application.Messaging.Behaviours;

/// <summary>
/// Validation decorator for command handlers. Validates commands using a
/// FluentValidation <see cref="AbstractValidator{T}"/> before delegating to
/// the inner handler. Returns an invalid response when validation fails.
/// </summary>
/// <typeparam name="TCommand">The command type to validate and handle.</typeparam>
public class ValidationCommandHandler<TCommand>
    (AbstractValidator<TCommand> validator, ICommandHandler<TCommand> inner) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Validates the command and either returns an invalid response with
    /// details or delegates to the inner handler.
    /// </summary>
    /// <param name="command">The command to validate and handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response"/> representing the outcome.</returns>
    public async Task<Response> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder();

        if (command == null)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("Request was not provided")
                .Build();
        }

        var validationResults = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResults.IsValid)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("One or more validation errors occured")
                .WithDetails(validationResults.AsErrors())
                .Build();
        }

        return await inner.Handle(command, cancellationToken);
    }
}

/// <summary>
/// Validation decorator for command handlers that return a typed result.
/// Validates commands using a FluentValidation <see cref="AbstractValidator{T}"/>
/// before delegating to the inner handler. Returns an invalid response when
/// validation fails.
/// </summary>
/// <typeparam name="TCommand">The command type to validate and handle.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
public class ValidationCommandHandler<TCommand, TResult>
    (AbstractValidator<TCommand> validator, ICommandHandler<TCommand, TResult> inner) : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    /// <summary>
    /// Validates the command and either returns an invalid response with
    /// details or delegates to the inner handler.
    /// </summary>
    /// <param name="command">The command to validate and handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response{TResult}"/> representing the outcome.</returns>
    public async Task<Response<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TResult>();

        if(command == null)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("Request was not provided")
                .Build();
        }

        var validationResults = await validator.ValidateAsync(command, cancellationToken);

        if(!validationResults.IsValid)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("One or more validation errors occured")
                .WithDetails(validationResults.AsErrors())
                .Build();
        }

        return await inner.Handle(command, cancellationToken);
    }
}
