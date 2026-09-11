using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Behaviours.QueryHandlerBehaviours;

public class LoggingQueryHandler<TQuery, TResult>
    (ILogger<TQuery> logger, IQueryHandler<TQuery, TResult> inner) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
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
