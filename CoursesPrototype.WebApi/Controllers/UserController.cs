using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.Exceptions;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response<string>> Login(string nickname, string password)
        {
            return await userInteractor.AuthenticateAsync(nickname, password);
        }

        [Authorize]
        [HttpPost("update-user")]
        public async Task<Response> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                string userNickname = GetAuthorizedUserNickname();

                return await userInteractor.UpdateUserAsync(userNickname, userDto);
            }
            catch (ForClientSideBaseException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
        }

        [Authorize]
        [HttpPost("update-pass")]
        public async Task<Response> UpdatePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                string userNickname = GetAuthorizedUserNickname();

                return await userInteractor.UpdateUserPasswordAsync(userNickname, userId, oldPassword, newPassword);
            }
            catch (ForClientSideBaseException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveAccountAsync(int userId)
        {
            try
            {
                string userNickname = GetAuthorizedUserNickname();

                return await userInteractor.RemoveUserAsync(userNickname, userId);
            }
            catch(ForClientSideBaseException exception)
            {
                return new Response()
                {
                    Success = false,
                    Message = exception.Message,
                };
            }
        }

        [AllowAnonymous]
        [HttpGet("auth-check")]
        public bool CheckAuthentication()
        {
            try
            {
                string userNickname = GetAuthorizedUserNickname();

                return true;
            }
            catch (ForClientSideBaseException)
            {
                return false;
            }
        }


        private string GetAuthorizedUserNickname()
        {
            if(User.Identity == null || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                throw new ForClientSideBaseException("Авторизованный пользователь не найден");
            }

            return User.Identity.Name;
        }
    }
}
