using PandaRedu.Api.Mappers.ResponseToResult.Abstractions;
using PandaRedu.Application.Shared.Responses;

namespace PandaRedu.Api.Mappers.ResponseToResult.Strategies;

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
