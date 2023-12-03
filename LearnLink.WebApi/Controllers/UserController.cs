using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly UserInteractor userInteractor;
        private readonly UserVerifierService userVerifierService;

        public UserController(UserInteractor userInteractor, UserVerifierService verifierService)
        {
            this.userInteractor = userInteractor;
            this.userVerifierService = verifierService;

        }

        [Authorize]
        [HttpGet("get")]
        public async Task<Response<UserDto>> GetUserAsync()
        {
            var nickname = User.Identity?.Name;

            return await userInteractor.GetUserByNicknameAsync(nickname);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(UserDto userDto, string password)
        {
            return await userInteractor.RegisterAsync(userDto, password);
        }
        
       
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response<string>> Login(string nickname, string password)
        {
            return await userInteractor.AuthenticateAsync(nickname, password);
        }

        
        [Authorize]
        [HttpPost("update-user")]
        public async Task<Response<string?>> UpdateUserAsync(UserDto userDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userDto.Id);

            if (!verifyResponse.Success)
            {
                return new()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await userInteractor.UpdateUserAsync(userDto);
        }

        [Authorize]
        [HttpPost("update-pass")]
        public async Task<Response> UpdatePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await userInteractor.UpdateUserPasswordAsync(userId, oldPassword, newPassword);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveAccountAsync(int userId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await userInteractor.RemoveUserAsync(userId);
        }
    }
}
