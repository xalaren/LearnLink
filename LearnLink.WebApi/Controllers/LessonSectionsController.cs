using LearnLink.Application.Interactors;
using LearnLink.Application.Mappers;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/lesson/sections")]
    public class LessonSectionsController : Controller
    {
        private readonly LessonSectionInteractor sectionInteractor;

        public LessonSectionsController(LessonSectionInteractor sectionInteractor)
        {
            this.sectionInteractor = sectionInteractor;
        }

        [Authorize]
        [HttpGet("get/fromlesson")]
        public async Task<Response<SectionDto[]>> GetSectionsFromLessonAsync(int lessonId)
        {
            return await sectionInteractor.GetFromLessonAsync(lessonId);
        }

        [Authorize]
        [HttpGet("get/by-lesson-order")]
        public async Task<Response> GetSectionByLessonAndOrder(int lessonId, int order)
        {
            return await sectionInteractor.GetSectionByLessonAndOrderAsync(lessonId, order);
        }


        [Authorize]
        [HttpPost("create/text")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionTextContentDto sectionTextDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.CreateLessonSectionAsync(lessonId, sectionTextDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("create/file")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionFileContentDto sectionFileDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.CreateLessonSectionAsync(lessonId, sectionFileDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("create/code")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionCodeContentDto sectionCodeDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.CreateLessonSectionAsync(lessonId, sectionCodeDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/order")]
        public async Task<Response> ChangeSectionOrderAsync(int sectionId, int lessonId, bool increase)
        {
            return await sectionInteractor.ChangeOrder(sectionId, lessonId, increase);
        }

        [Authorize]
        [HttpPost("update/text")]
        public async Task<Response> UpdateLessonSectionAsync([FromForm] SectionTextContentDto sectionTextDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.UpdateLessonSectionAsync(lessonId, sectionTextDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/file")]
        public async Task<Response> UpdateLessonSectionAsync([FromForm] SectionFileContentDto sectionFileDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.UpdateLessonSectionAsync(lessonId, sectionFileDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/code")]
        public async Task<Response> UpdateLessonSectionAsync([FromForm] SectionCodeContentDto sectionCodeDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.UpdateLessonSectionAsync(lessonId, sectionCodeDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateLessonSectionAsync([FromForm] SectionDto sectionDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.UpdateLessonSectionAsync(lessonId, sectionDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveLessonSectionAsync(int lessonId, int sectionId)
        {
            return await sectionInteractor.RemoveLessonSectionAsync(sectionId, lessonId);
        }
    }
}
