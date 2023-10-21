using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/Modules")]
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

        //TODO: make private
        [HttpGet("get-all")]
        public async Task<Response<ModuleDto[]>> GetAllModules()
        {
            return await moduleInteractor.GetAllModulesAsync();
        }

        [HttpGet("get-course-modules")]
        public async Task<Response<ModuleDto[]>> GetCourseModulesAsync(int courseId)
        {
            return await moduleInteractor.GetCourseModulesAsync(courseId);
        }

        [HttpPost("create")]
        public async Task<Response> CreateModuleAsync(int courseId, ModuleDto moduleDto)
        {
            return await moduleInteractor.CreateModuleAsync(courseId, moduleDto);
        }

        [HttpPost("update")]
        public async Task<Response> UpdateModuleAsync(ModuleDto moduleDto)
        {
            return await moduleInteractor.UpdateModuleAsync(moduleDto);
        }

        [HttpDelete("remove")]
        public async Task<Response> RemoveModuleAsync(int moduleId)
        {
            return await moduleInteractor.RemoveModuleAsync(moduleId);
        }

    }
}
