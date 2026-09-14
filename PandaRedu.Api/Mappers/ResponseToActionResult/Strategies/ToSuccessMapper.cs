using PandaRedu.Api.Mappers.ResponseToActionResult.Abstractions;
using PandaRedu.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace PandaRedu.Api.Mappers.ResponseToActionResult.MappingStrategies;

public class ToSuccessMapper : IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller)
    {
        return controller.Ok(response.Message);
    }
}
