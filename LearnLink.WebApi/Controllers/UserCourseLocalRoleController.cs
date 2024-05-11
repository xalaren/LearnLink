using LearnLink.Application.Interactors;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/roles/userCourseLocal")]
    public class UserCourseLocalRoleController(
        UserVerifierService userVerifierService,
        UserCourseLocalRolesInteractor userCourseLocalRolesInteractor)
        : Controller
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<LocalRoleDto>> GetLocalRoleByUserCourse(int userId, int courseId)
        {
            return await userCourseLocalRolesInteractor.GetLocalRoleByUserCourse(userId, courseId);
        }
        
        [Authorize]
        [HttpPost("request/reassign")]
        public async Task<Response> RequestReassignUserRoleAsync(int requesterUserId, int targetUserId, int courseId, int localRoleId)
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

            return await userCourseLocalRolesInteractor.RequestReassignUserRoleAsync(requesterUserId, targetUserId, courseId, localRoleId);
        }
    }
}
