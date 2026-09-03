using Ardalis.Result.AspNetCore;
using LearnLink.Application.Services;
using LearnLink.Shared.Model.Users;
using LearnLink.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request, [FromQuery] string password)
    {
       return (await _userService.RegisterAsync(request, password)).ToActionResult(this);
    }

    [HttpGet("list")]
    public async Task<ActionResult<PagedResponse<UserDto>>> List([FromBody] ListRequest request)
    {
        return (await _userService.ListAsync(request)).ToActionResult(this);
    }


}
