using RustyTail.Api.Mappers.ResponseToResult.Abstractions;
using RustyTail.Application.Shared.Responses;

namespace RustyTail.Api.Mappers.ResponseToResult.Strategies;

public class ToInternalServerErrorMapper : IResponseMapper
{
    public IResult Map(Response response)
    {
        return Results.Problem
        (
            statusCode: StatusCodes.Status500InternalServerError,
            title: response.Message,
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
        );
    }
}
