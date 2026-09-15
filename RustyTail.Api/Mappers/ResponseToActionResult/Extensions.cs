using RustyTail.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Mappers.ResponseToActionResult;

public static class Extensions
{
    /// <summary>
    /// Maps an application <see cref="Response"/> to an ASP.NET Core <see cref="ActionResult"/>.
    /// </summary>
    public static ActionResult ToActionResult(this Response response, ControllerBase controller)
    {
        var context = new MapContext();
        return context.Execute(response, controller);
    }

    /// <summary>
    /// Maps a generic <see cref="Response{TContent}"/> to an <see cref="ActionResult"/>,
    /// returning the content for successful responses.
    /// </summary>
    public static ActionResult ToActionResult<TContent>(this Response<TContent> response, ControllerBase controller)
    {
        if(response.IsSuccess)
        {
            return controller.Ok(response.Content);
        }

        return ToActionResult(response as Response, controller);
    }
}
