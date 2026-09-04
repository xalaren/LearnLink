using Ardalis.Result.AspNetCore;
using LearnLink.Application.Services;
using LearnLink.Shared.Model.Users;
using LearnLink.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ApiControllerBase
{
    private readonly UserService _userService = userService;
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult> Register(RegisterRequest request, [FromQuery] string password)
    {
       return (await _userService.RegisterAsync(request, password)).ToActionResult(this);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<PagedResponse<UserDto>>> List([FromBody] ListRequest request)
    {
        return (await _userService.ListAsync(request)).ToActionResult(this);
    }


}
