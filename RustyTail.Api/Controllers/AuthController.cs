using RustyTail.Api.Mappers.ResponseToActionResult;
using RustyTail.Application.Abstractions.Messaging;
using RustyTail.Application.Security.Commands;
using RustyTail.Application.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Controllers;

/// <summary>
/// Controller responsible for authentication endpoints such as login and token refresh.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ApiControllerBase 
{
    /// <summary>
    /// Authenticates a user by nickname and password and returns a token pair on success.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<TokenPair>> Login(
        [FromBody] LoginCommand command,
        [FromServices] ICommandHandler<LoginCommand, TokenPair> handler,
        CancellationToken cancellationToken = default)
    {
        var response = await handler.Handle(command, cancellationToken);
        return response.ToActionResult(this);
    }

    /// <summary>
    /// Exchanges a refresh token for a new access/refresh token pair.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<TokenPair>> Refresh(
        [FromBody] RefreshCommand command,
        [FromServices] ICommandHandler<RefreshCommand, TokenPair> handler,
        CancellationToken cancellationToken = default)
    {
        var response = await handler.Handle(command, cancellationToken);
        return response.ToActionResult(this);
    }

    /// <summary>
    /// Log out user
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult> Logout (
        [FromBody] LogoutCommand command,
        [FromServices] ICommandHandler<LogoutCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var response = await handler.Handle(command, cancellationToken);
        return response.ToActionResult(this);
    }
}
