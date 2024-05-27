using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/completions")]
    public class CompletionController(CompletionInteractor completionInteractor, UserVerifierService verifierService) : Controller
    {
        [Authorize]
        [HttpGet("get/course-completion")]
        public async Task<Response<CourseCompletionDto>> GetCourseCompletionAsync(int userId, int courseId)
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
            
            return await completionInteractor.GetCourseCompletion(userId, courseId);
        }

        [Authorize]
        [HttpGet("get/module-completions")]
        public async Task<Response<ModuleCompletionDto[]>> GetModuleCompletionsOfCourseAsync(int userId, int courseId)
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
            
            return await completionInteractor.GetModuleCompletionsOfCourseAsync(userId, courseId);
        }

        [Authorize]
        [HttpGet("get/lesson-completions")]
        public async Task<Response<LessonCompletionDto[]>> GetLessonCompletionsOfModuleAsync(int userId, int moduleId)
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
            
            return await completionInteractor.GetLessonCompletionsOfModuleAsync(userId, moduleId);
        }

        [Authorize]
        [HttpPost("complete/lesson")]
        public async Task<Response> CompleteLessonAsync(int userId, int courseId, int moduleId, int lessonId, bool complete)
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

            return await completionInteractor.ChangeLessonCompleted(userId, courseId, moduleId, lessonId, complete);
        }
    }
}
