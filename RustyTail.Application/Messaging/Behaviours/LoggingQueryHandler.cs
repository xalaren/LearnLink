using RustyTail.Application.Abstractions.Messaging;
using RustyTail.Application.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace RustyTail.Application.Messaging.Behaviours;

/// <summary>
/// Logging decorator for query handlers. Logs processing start, end and
/// errors for queries handled by the inner handler.
/// </summary>
/// <typeparam name="TQuery">The query type being handled.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
public class LoggingQueryHandler<TQuery, TResult>
    (ILogger<TQuery> logger, IQueryHandler<TQuery, TResult> inner) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    /// <summary>
    /// Processes the query by logging progress, delegating to the inner
    /// handler and logging the result or any unhandled exception.
    /// </summary>
    /// <param name="query">The query to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response{TResult}"/> representing the outcome.</returns>
    public async Task<Response<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType().Name;
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Query {Query} processing...", queryType);

            var response = await inner.Handle(query, cancellationToken);

            if (!response.IsSuccess)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Query {Query} process ended with errors", queryType);
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError("Query {Query} process ended with errors. Details:\n{Response}", queryType, response);
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Query {Query} process ended with success", queryType);
            }

            return response;
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Query {Query} process ended with errors", queryType);
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError(ex, "Unhandled exception handling {Query}", queryType);

            return new ResponseBuilder<TResult>()
                .Fail()
                .WithMessage("Unknown error occured")
                .Build();
        }
    }
}
