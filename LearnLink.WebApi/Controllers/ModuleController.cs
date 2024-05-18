using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/modules")]
    public class ModuleController(ModuleInteractor moduleInteractor, UserVerifierService verifierService) : Controller
    {
        [HttpGet("get")]
        public async Task<Response<ClientModuleDto?>> RequestGetModuleAsync(int userId, int courseId, int moduleId)
        {
            var response = await verifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!response.Success)
            {
                return new()
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                };
            }

            return await moduleInteractor.RequestGetModuleAsync(userId, courseId, moduleId);
        }

        [HttpGet("get/atCourse")]
        public async Task<Response<ClientModuleDto[]>> RequestGetCourseModulesAsync(int userId, int courseId)
        {
            return await moduleInteractor.RequestGetCourseModulesAsync(courseId, userId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> RequestCreateModuleAsync(int userId, int courseId, ModuleDto moduleDto)
        {
            var response = await verifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!response.Success)
            {
                return new()
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                };
            }

            return await moduleInteractor.RequestCreateModuleAsync(userId, courseId, moduleDto);
        }


        [Authorize]
        [HttpPost("update")]
        public async Task<Response> RequestUpdateModuleAsync(int userId, int courseId, ModuleDto moduleDto)
        {
            var response = await verifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!response.Success)
            {
                return new()
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                };
            }
            return await moduleInteractor.RequestUpdateModuleAsync(userId, courseId, moduleDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RequestRemoveModuleAsync(int userId, int courseId, int moduleId)
        {
            var response = await verifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!response.Success)
            {
                return new()
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Message = response.Message,
                };
            }

            return await moduleInteractor.RequestRemoveModuleAsync(userId, courseId, moduleId);
        }

    }
}
