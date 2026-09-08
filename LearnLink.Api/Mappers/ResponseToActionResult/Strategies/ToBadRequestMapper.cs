using LearnLink.Api.Mappers.ResponseToActionResult.Abstractions;
using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult.MappingStrategies;

public class ToBadRequestMapper : IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller)
    {
        return controller.Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: response.Message,
            type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", response.Details }
            }
        );
    }
}
