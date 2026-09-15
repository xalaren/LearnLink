using RustyTail.Api.Mappers.ResponseToResult.Abstractions;
using RustyTail.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Mappers.ResponseToResult.Strategies;

public class ToNotFoundMapper : IResponseMapper
{
    public IResult Map(Response response)
    {
        return Results.Problem
        (
            statusCode: StatusCodes.Status404NotFound,
            title: response.Message,
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
        );
    }
}
