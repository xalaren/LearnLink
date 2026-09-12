using LearnLink.Application.Shared.Responses;

namespace LearnLink.Application.Abstractions.Messaging;

/// <summary>
/// Handles a query of type <typeparamref name="TQuery"/> and returns a
/// <see cref="Response{TResult}"/> containing the requested data.
/// </summary>
/// <typeparam name="TQuery">The query type to handle.</typeparam>
/// <typeparam name="TResult">The type of the result returned in the response.</typeparam>
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    /// <summary>
    /// Processes the specified query and returns a typed response.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the handling operation. The task result contains a <see cref="Response{TResult}"/>.</returns>
    Task<Response<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
