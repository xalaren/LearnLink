using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/completions")]
    public class CompletionController : Controller
    {
        private readonly CompletionInteractor completionInteractor;

        public CompletionController(CompletionInteractor completionInteractor)
        {
            this.completionInteractor = completionInteractor;
        }

        [Authorize]
        [HttpGet("get/course-completion")]
        public async Task<Response<CourseCompletionDto>> GetCourseCompletionAsync(int userId, int courseId)
        {
            return await completionInteractor.GetCourseCompletion(userId, courseId);
        }

        [Authorize]
        [HttpGet("get/module-completions")]
        public async Task<Response<ModuleCompletionDto[]>> GetModuleCompletionsOfCourseAsync(int userId, int courseId)
        {
            return await completionInteractor.GetModuleCompletionsOfCourseAsync(userId, courseId);
        }

        [Authorize]
        [HttpGet("get/lesson-completions")]
        public async Task<Response<LessonCompletionDto[]>> GetLessonCompletionsOfModuleAsync(int userId, int moduleId)
        {
            return await completionInteractor.GetLessonCompletionsOfModuleAsync(userId, moduleId);
        }
    }
}
