﻿using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{

    [ApiController]
    [Route("api/roles/courseLocal")]
    public class CourseLocalRoleController(
        CourseLocalRoleInteractor courseLocalRoleInteractor,
        UserVerifierService userVerifierService)
        : Controller
    {
        [Authorize]
        [HttpPost("request/create")]
        public async Task<Response> CreateAsync(int requesterUserId, int courseId, LocalRoleDto localRoleDto)
        {
            return await courseLocalRoleInteractor.RequestCreateAsync(requesterUserId, courseId, localRoleDto);
        }

        [Authorize]
        [HttpGet("get/atCourse")]
        public async Task<Response<LocalRoleDto[]>> GetLocalRolesAtCourseAsync(int courseId)
        {
            return await courseLocalRoleInteractor.GetLocalRolesAtCourseAsync(courseId);
        }

        [Authorize]
        [HttpPost("request/update")]
        public async Task<Response> RequestUpdateLocalRoleAsync(int requesterUserId, int courseId, LocalRoleDto localRoleDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, requesterUserId);

            if (!verifyResponse.Success)
            {
                return new()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await courseLocalRoleInteractor.RequestUpdateLocalRoleAsync(requesterUserId, courseId, localRoleDto);
        }

        [Authorize]
        [HttpDelete("request/delete")]
        public async Task<Response> RequestRemoveAsync(int requesterUserId, int courseId, int localRoleId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, requesterUserId);

            if (!verifyResponse.Success)
            {
                return new()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await courseLocalRoleInteractor.RequestRemoveAsync(requesterUserId, courseId, localRoleId);
        }


    }
}
