using PandaRedu.Application.Shared.Responses;

namespace PandaRedu.Api.Mappers.ResponseToResult;

public static class Extensions
{
    /// <summary>
    /// Maps an application <see cref="Response"/> to a minimal API <see cref="IResult"/>.
    /// </summary>
    public static IResult ToResult(this Response response)
    {
        var context = new MapContext();
        return context.Execute(response);
    }

    /// <summary>
    /// Maps a generic <see cref="Response{TContent}"/> to an <see cref="IResult"/>,
    /// returning the content for successful responses.
    /// </summary>
    public static IResult ToResult<TContent>(this Response<TContent> response)
    {
        if(response.IsSuccess)
        {
            return Results.Ok(response.Content);
        }

        return ToResult(response);
    }
}
