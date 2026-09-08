using LearnLink.Api.Mappers.ResponseToResult.Abstractions;
using LearnLink.Application.Shared.Responses;

namespace LearnLink.Api.Mappers.ResponseToResult.Strategies;

public class ToSuccessMapper : IResponseMapper
{
    public IResult Map(Response response)
    {
        return Results.Ok
        (
            response.Message
        );
    }
}
