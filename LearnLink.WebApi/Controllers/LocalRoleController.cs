using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/roles/local")]
    public class LocalRoleController : Controller
    {
        private readonly LocalRoleInteractor localRoleInteractor;

        public LocalRoleController(LocalRoleInteractor localRoleInteractor)
        {
            this.localRoleInteractor = localRoleInteractor;
        }

        [Authorize]
        [HttpGet("get/all")]
        public async Task<Response<LocalRoleDto[]>> GetAllLocalRolesAsync()
        {
            return await localRoleInteractor.GetAllLocalRolesAsync();
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<Response<LocalRoleDto?>> GetLocalRoleByIdAsync(int roleId)
        {
            return await localRoleInteractor.GetLocalRoleByIdAsync(roleId);
        }

        [Authorize]
        [HttpGet("get/by-name")]
        public async Task<Response<LocalRoleDto?>> GetLocalRoleByNameAsync(string name)
        {
            return await localRoleInteractor.GetLocalRoleByNameAsync(name);
        }

        [Authorize]
        [HttpGet("get/byUserAtCourse")]
        public async Task<Response<LocalRoleDto>> GetLocalRoleByUserAtCourseAsync(int courseId, int userId)
        {
            return await localRoleInteractor.GetUserLocalRoleAtCourse(courseId, userId);
        }


        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateLocalRoleAsync(LocalRoleDto LocalRoleDto)
        {
            return await localRoleInteractor.CreateLocalRoleAsync(LocalRoleDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateLocalRoleAsync(LocalRoleDto LocalRoleDto)
        {
            return await localRoleInteractor.UpdateLocalRoleAsync(LocalRoleDto);
        }

        [Authorize]
        [HttpPost("reassign")]
        public async Task<Response> ReassignLocalRoleAsync(int requesterUserId, int targetUserId, int courseId, int localRoleId)
        {
            return await localRoleInteractor.ReassignUserRoleAsync(requesterUserId, targetUserId, courseId, localRoleId);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveLocalRoleAsync(int roleId)
        {
            return await localRoleInteractor.RemoveLocalRoleAsync(roleId);
        }

    }
}
