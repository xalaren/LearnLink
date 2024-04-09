using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : Controller
    {
        private readonly LessonInteractor lessonInteractor;

        public LessonController(LessonInteractor lessonInteractor)
        {
            this.lessonInteractor = lessonInteractor;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<Response<LessonDto>> GetLessonAsync(int lessonId)
        {
            return await lessonInteractor.GetLessonAsync(lessonId);
        }

        [Authorize]
        [HttpGet("get/all")]
        public async Task<Response<LessonDto[]>> GetAllLessonsAsync()
        {
            return await lessonInteractor.GetAllLessonsAsync();
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateLessonAsync(int courseId, int moduleId, LessonDto lessonDto)
        {
            return await lessonInteractor.CreateLessonAsync(courseId, moduleId, lessonDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateLessonAsync(LessonDto lessonDto)
        {
            return await lessonInteractor.UpdateLessonAsync(lessonDto);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<Response> RemoveLessonAsync(int lessonId)
        {
            return await lessonInteractor.RemoveLessonAsync(lessonId);
        }
    }
}
