using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult.Abstractions;

public interface IResponseMapper
{
    public ActionResult Map(Response response, ControllerBase controller);
}
