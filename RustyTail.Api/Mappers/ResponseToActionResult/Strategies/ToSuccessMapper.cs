using RustyTail.Api.Mappers.ResponseToActionResult.Abstractions;
using RustyTail.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Mappers.ResponseToActionResult.MappingStrategies;

public class ToSuccessMapper : IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller)
    {
        return controller.Ok(response.Message);
    }
}
