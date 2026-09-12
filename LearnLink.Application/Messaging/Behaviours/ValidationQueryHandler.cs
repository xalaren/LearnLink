using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Extensions;

namespace LearnLink.Application.Messaging.Behaviours;

/// <summary>
/// Validation decorator for query handlers. Validates queries using a
/// FluentValidation <see cref="AbstractValidator{T}"/> before delegating to
/// the inner handler. Returns an invalid response when validation fails.
/// </summary>
/// <typeparam name="TQuery">The query type to validate and handle.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
public class ValidationQueryHandler<TQuery, TResult>
    (AbstractValidator<TQuery> validator, IQueryHandler<TQuery, TResult> inner) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    /// <summary>
    /// Validates the query and either returns an invalid response with
    /// details or delegates to the inner handler.
    /// </summary>
    /// <param name="query">The query to validate and handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response{TResult}"/> representing the outcome.</returns>
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
