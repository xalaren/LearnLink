using Ardalis.Result.AspNetCore;
using LearnLink.Application.Services;
using LearnLink.Shared.Users;
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
        var result = await _userService.RegisterAsync(request, password);
        return result.ToActionResult(this);
    }
}
