using LearnLink.Api.Mappers.ResponseToActionResult;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Users.Commands;
using LearnLink.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult> Register(
        [FromBody] RegisterCommand command,
        [FromServices] ICommandHandler<RegisterCommand> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return response.ToActionResult(this);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListQueryResult), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<ListQueryResult>> List(
        [FromQuery] ListQuery query,
        [FromServices] IQueryHandler<ListQuery, ListQueryResult> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(query, cancellationToken);
        return response.ToActionResult(this);
    }
}
