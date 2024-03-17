using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
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

        [HttpGet("get-all")]
        public async Task<Response<LocalRoleDto[]>> GetAllLocalRolesAsync()
        {
            return await localRoleInteractor.GetAllLocalRolesAsync();
        }

        [HttpGet("get")]
        public async Task<Response<LocalRoleDto?>> GetLocalRoleByIdAsync(int roleId)
        {
            return await localRoleInteractor.GetLocalRoleByIdAsync(roleId);
        }

        [HttpGet("get-by-name")]
        public async Task<Response<LocalRoleDto?>> GetLocalRoleByNameAsync(string name)
        {
            return await localRoleInteractor.GetLocalRoleByNameAsync(name);
        }

        [HttpPost("create")]
        public async Task<Response> CreateLocalRoleAsync(LocalRoleDto LocalRoleDto)
        {
            return await localRoleInteractor.CreateLocalRoleAsync(LocalRoleDto);
        }

        [HttpPost("update")]
        public async Task<Response> UpdateLocalRoleAsync(LocalRoleDto LocalRoleDto)
        {
            return await localRoleInteractor.UpdateLocalRoleAsync(LocalRoleDto);
        }

        [HttpDelete("remove")]
        public async Task<Response> RemoveLocalRoleAsync(int roleId)
        {
            return await localRoleInteractor.RemoveLocalRoleAsync(roleId);
        }

    }
}
