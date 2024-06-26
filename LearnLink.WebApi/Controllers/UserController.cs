﻿using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserInteractor userInteractor;
        private readonly UserVerifierService userVerifierService;

        public UserController(UserInteractor userInteractor, UserVerifierService userVerifierService)
        {
            this.userInteractor = userInteractor;
            this.userVerifierService = userVerifierService;
        }

        [Authorize]
        [HttpGet("get/auth")]
        public async Task<Response<UserDto>> GetAuthenticatedUser()
        {
            var nickname = User.Identity?.Name;

            var response = await userInteractor.GetUserByNicknameAsync(nickname);

            return response;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<Response<UserDto>> GetUserAsync(int userId)
        {
            var response = await userInteractor.GetUserAsync(userId);
            return response;
        }

        [Authorize]
        [HttpGet("find/exceptCourseUsers")]
        public async Task<Response<DataPage<UserDto[]>>> FindUsersExcludeCourseAsync(int courseId, string? searchText, int page, int size)
        {
            return await userInteractor.FindUsersExceptCourseUsersAsync(courseId, searchText, new DataPageHeader(page, size));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> RegisterAsync([FromForm] UserDto userDto, [FromQuery] string password)
        {
            return await userInteractor.RegisterAsync(userDto, password);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response> Login(string nickname, string password)
        {
            return await userInteractor.AuthenticateAsync(nickname, password);
        }


        [Authorize]
        [HttpPost("update/user")]
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
        [HttpPost("update/pass")]
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
