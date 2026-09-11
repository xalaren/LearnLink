using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Extensions;

namespace LearnLink.Application.Behaviours.QueryHandlerBehaviours;

public class ValidationQueryHandler<TQuery, TResult>
    (AbstractValidator<TQuery> validator, IQueryHandler<TQuery, TResult> inner) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    public async Task<Response<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TResult>();

        if(query == null)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("Request was not provided")
                .Build();
        }

        var validationResults = await validator.ValidateAsync(query, cancellationToken);

        if(!validationResults.IsValid)
        {
            return responseBuilder
                .Invalid()
                .WithMessage("One or more validation errors occured")
                .WithDetails(validationResults.AsErrors())
                .Build();
        }

        return await inner.Handle(query, cancellationToken);
    }
}
