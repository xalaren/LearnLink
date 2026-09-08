using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult;

public static class Extensions
{
    public static ActionResult ToActionResult(this Response response, ControllerBase controller)
    {
        var context = new MapContext();
        return context.Execute(response, controller);
    }

    public static ActionResult ToActionResult<TContent>(this Response<TContent> response, ControllerBase controller)
    {
        if(response.IsSuccess)
        {
            return controller.Ok(response.Content);
        }

        return ToActionResult(response, controller);
    }
}
