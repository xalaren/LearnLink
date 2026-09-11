using LearnLink.Application.Messaging.Abstractions;
using LearnLink.Application.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Messaging.QueryHandlersBehaviours;


public class LoggingQueryHandler<TQuery, TResult>
    (ILogger<TQuery> logger, IQueryHandler<TQuery, TResult> inner) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    public async Task<Response<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType().Name;
        try
        {
            if(logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Query {Query} processing...", queryType);
            }

            var response = await inner.Handle(query, cancellationToken);

            if (!response.IsSuccess)
            {
                logger.LogInformation("Query {Query} process ended with errors", queryType);
                logger.LogError("Query {Query} process ended with errors. Details:\n{Response}", queryType, response);
            }
            else
            {
                logger.LogInformation("Query {Query} process ended with success", queryType);
            }

            return response;
        }
        catch (Exception ex)
        {
            logger.LogInformation("Query {Query} process ended with errors", queryType);
            logger.LogError(ex, "Unhandled exception handling {Query}", queryType);

            return new ResponseBuilder<TResult>()
                .Fail()
                .WithMessage("Unknown error occured")
                .Build();
        }
    }
}
