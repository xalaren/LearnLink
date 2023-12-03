using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/Roles")]
    public class RoleController : Controller
    {
        private readonly RoleInteractor roleInteractor;

        public RoleController(RoleInteractor roleInteractor)
        {
            this.roleInteractor = roleInteractor;
        }

        [HttpGet("get-all")]
        public async Task<Response<RoleDto[]>> GetAllRolesAsync()
        {
            return await roleInteractor.GetAllRolesAsync();
        }

        [HttpGet("get")]
        public async Task<Response<RoleDto?>> GetRoleByIdAsync(int roleId)
        {
            return await roleInteractor.GetRoleByIdAsync(roleId);
        }

        [HttpGet("get-by-name")]
        public async Task<Response<RoleDto?>> GetRoleByNameAsync(string name)
        {
            return await roleInteractor.GetRoleByNameAsync(name);
        }

        [HttpPost("create")]
        public async Task<Response> CreateRoleAsync(RoleDto roleDto)
        {
            return await roleInteractor.CreateRoleAsync(roleDto);
        }

        [HttpPost("allocate")]
        public async Task<Response> AllocateRoleToUserAsync(int roleId, int userId)
        {
            return await roleInteractor.AllocateRoleToUserAsync(roleId, userId);
        }

        [HttpPost("update")]
        public async Task<Response> UpdateRoleAsync(RoleDto roleDto)
        {
            return await roleInteractor.UpdateRoleAsync(roleDto);
        }

        [HttpDelete("remove")]
        public async Task<Response> RemoveRoleAsync(int roleId)
        {
            return await roleInteractor.RemoveRoleAsync(roleId);
        }

    }
}
