using LearnLink.Api.Mappers.ResponseToActionResult;
using LearnLink.Application.Security.Models;
using LearnLink.Application.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AuthenticationService authenticationService): ApiControllerBase 
    {
        private readonly AuthenticationService _authenticationService = authenticationService;

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenPair), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<TokenPair>> Login(string nickname, string password)
        {
            return (await _authenticationService.LoginAsync(nickname, password)).ToActionResult(this);
        }
    }
}
