using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult.Abstractions;

/// <summary>
/// Maps application-level <see cref="Response"/> instances to ASP.NET Core
/// <see cref="ActionResult"/> values.
/// </summary>
public interface IResponseMapper
{
    /// <summary>
    /// Maps the provided <see cref="Response"/> to an <see cref="ActionResult"/> using the controller context.
    /// </summary>
    public ActionResult Map(Response response, ControllerBase controller);
}
