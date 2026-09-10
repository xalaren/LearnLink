using LearnLink.Application.Users.Models;
using LearnLink.Application.Users.Services;
using LearnLink.Application.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnLink.Api.Mappers.ResponseToActionResult;

namespace LearnLink.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ApiControllerBase
{
    private readonly UserService _userService = userService;
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult> Register(RegisterRequest request, CancellationToken cancellationToken = default)
    {
       return (await _userService.RegisterAsync(request, cancellationToken)).ToActionResult(this);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedContent<UserDto>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<PagedContent<UserDto>>> List(ListRequest request, CancellationToken cancellationToken = default)
    {
        return (await _userService.ListAsync(request, cancellationToken)).ToActionResult(this);
    }
}
