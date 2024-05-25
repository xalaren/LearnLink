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
        [HttpPost("create")]
        public async Task<Response> CreateLocalRoleAsync(LocalRoleDto localRoleDto)
        {
            return await localRoleInteractor.CreateLocalRoleAsync(localRoleDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateLocalRoleAsync(LocalRoleDto localRoleDto)
        {
            return await localRoleInteractor.UpdateLocalRoleAsync(localRoleDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveLocalRoleAsync(int roleId)
        {
            return await localRoleInteractor.RemoveLocalRoleAsync(roleId);
        }

    }
}
