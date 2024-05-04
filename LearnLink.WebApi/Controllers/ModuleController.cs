using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/modules")]
    public class ModuleController
    {
        private readonly ModuleInteractor moduleInteractor;

        public ModuleController(ModuleInteractor moduleInteractor)
        {
            this.moduleInteractor = moduleInteractor;
        }


        [HttpGet("get")]
        public async Task<Response<ModuleDto?>> GetModule(int moduleId)
        {
            return await moduleInteractor.GetModuleAsync(moduleId);
        }

        [HttpGet("get/all")]
        public async Task<Response<ModuleDto[]>> GetAllModules()
        {
            return await moduleInteractor.GetAllModulesAsync();
        }

        [HttpGet("get/atCourse")]
        public async Task<Response<ClientModuleDto[]>> GetCourseModulesAsync(int courseId, int userId)
        {
            return await moduleInteractor.GetCourseModulesAsync(courseId, userId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateModuleAsync(int courseId, ModuleDto moduleDto)
        {
            return await moduleInteractor.CreateModuleAsync(courseId, moduleDto);
        }


        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateModuleAsync(ModuleDto moduleDto)
        {
            return await moduleInteractor.UpdateModuleAsync(moduleDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveModuleAsync(int moduleId)
        {
            return await moduleInteractor.RemoveModuleAsync(moduleId);
        }

    }
}
