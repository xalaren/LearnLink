using PandaRedu.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace PandaRedu.Api.Controllers;

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
/// <summary>
/// Base controller that configures common response types produced by API actions.
/// Other controllers should inherit from this class to apply standard
/// <see cref="ProducesResponseTypeAttribute"/> attributes for common HTTP status codes.
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{

}
