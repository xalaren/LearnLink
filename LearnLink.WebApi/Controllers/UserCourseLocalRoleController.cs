using LearnLink.Application.Interactors;
using LearnLink.Core.Entities;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/roles/userCourseLocal")]
    public class UserCourseLocalRoleController : Controller
    {
        private readonly UserCourseLocalRolesInteractor userCourseLocalRolesInteractor;
        private readonly UserVerifierService userVerifierService;

        public UserCourseLocalRoleController(UserVerifierService userVerifierService, UserCourseLocalRolesInteractor userCourseLocalRolesInteractor)
        {
            this.userVerifierService = userVerifierService;
            this.userCourseLocalRolesInteractor = userCourseLocalRolesInteractor;
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
