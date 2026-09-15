using RustyTail.Api.Mappers.ResponseToActionResult.Abstractions;
using RustyTail.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Mappers.ResponseToActionResult.MappingStrategies;

public class ToInternalServerErrorMapper : IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller)
    {
        return controller.Problem
        (
            statusCode: StatusCodes.Status500InternalServerError,
            title: response.Message,
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
        );
    }
}
