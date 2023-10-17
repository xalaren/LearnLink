using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;
using CoursesPrototype.Shared.ToClientData.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly UserInteractor userInteractor;

        public UserController(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(UserDto userDto, string password)
        {
            return await userInteractor.RegisterAsync(userDto, password);
        }

        [Authorize]
        [HttpGet("get-all")]
        public async Task<Response<UserDto[]>> GetAllAsync()
        {
            return await userInteractor.GetUsersAsync();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response<string>> Login(string nickname, string password)
        {
            return await userInteractor.AuthenticateAsync(nickname, password);
        }
    }
}
