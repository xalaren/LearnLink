using RustyTail.Api.Mappers.ResponseToResult.Abstractions;
using RustyTail.Application.Shared.Responses;

namespace RustyTail.Api.Mappers.ResponseToResult.Strategies;

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
