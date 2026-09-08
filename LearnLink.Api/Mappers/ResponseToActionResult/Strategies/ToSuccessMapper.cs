using LearnLink.Api.Mappers.ResponseToActionResult.Abstractions;
using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult.MappingStrategies;

public class ToSuccessMapper : IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller)
    {
        return controller.Ok(response.Message);
    }
}
