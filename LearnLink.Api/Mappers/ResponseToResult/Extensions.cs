using LearnLink.Application.Shared.Responses;

namespace LearnLink.Api.Mappers.ResponseToResult;

public static class Extensions
{
    public static IResult ToResult(this Response response)
    {
        var context = new MapContext();
        return context.Execute(response);
    }

    public static IResult ToResult<TContent>(this Response<TContent> response)
    {
        if(response.IsSuccess)
        {
            return Results.Ok(response.Content);
        }

        return ToResult(response);
    }
}
