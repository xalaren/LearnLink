using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Extensions;

namespace LearnLink.Application.Behaviours.CommandHandlerBehaviours;

public class ValidationCommandHandler<TCommand>
    (AbstractValidator<TCommand> validator, ICommandHandler<TCommand> inner) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
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

public class ValidationCommandHandler<TCommand, TResult>
    (AbstractValidator<TCommand> validator, ICommandHandler<TCommand, TResult> inner) : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
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
