using LearnLink.Api.Mappers.ResponseToActionResult;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Security.Commands;
using LearnLink.Application.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ApiControllerBase 
{

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
}
