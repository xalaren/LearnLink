using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController(LessonInteractor lessonInteractor, UserVerifierService verifierService) : Controller
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<ClientLessonDto>> RequestGetLessonAsync(int userId, int courseId, int lessonId)
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

            return await lessonInteractor.RequestGetLessonAsync(userId, courseId, lessonId);
        }

        [Authorize]
        [HttpGet("get/atModule")]
        public async Task<Response<ClientLessonDto[]>> RequestGetLessonsAtModuleAsync(int userId, int courseId, int moduleId)
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
            return await lessonInteractor.RequestGetModuleLessonsAsync(userId, courseId, moduleId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateLessonAsync(int userId, int courseId, int moduleId, LessonDto lessonDto)
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

            return await lessonInteractor.RequestCreateLessonAsync(userId, courseId, moduleId, lessonDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateLessonAsync(int userId, int courseId, LessonDto lessonDto)
        {
            return await lessonInteractor.RequestUpdateLessonAsync(userId, courseId, lessonDto);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<Response> RemoveLessonAsync(int userId, int courseId, int moduleId, int lessonId)
        {
            return await lessonInteractor.RequestRemoveLessonAsync(userId, courseId, moduleId, lessonId);
        }
    }
}
