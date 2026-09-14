using PandaRedu.Api.Mappers.ResponseToActionResult;
using PandaRedu.Application.Abstractions.Messaging;
using PandaRedu.Application.Users.Commands;
using PandaRedu.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PandaRedu.Api.Controllers;

/// <summary>
/// Controller that exposes user-related endpoints such as registration and listing users.
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController : ApiControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
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

    /// <summary>
    /// Returns a paged list of users. Requires authorization.
    /// </summary>
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
