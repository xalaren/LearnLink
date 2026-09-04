using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers;

[ProducesResponseType(typeof(Result<object>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Result<object>), StatusCodes.Status500InternalServerError)]
public abstract class ApiControllerBase : ControllerBase
{

}
