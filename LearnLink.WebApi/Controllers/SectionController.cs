using LearnLink.Application.Interactors;
using LearnLink.Application.Mappers;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/sections")]
    public class SectionController : Controller
    {
        private readonly SectionInteractor sectionInteractor;

        public SectionController(SectionInteractor sectionInteractor)
        {
            this.sectionInteractor = sectionInteractor;
        }

        [Authorize]
        [HttpGet("get/fromlesson")]
        public async Task<Response<SectionDto[]>> GetSectionsFromLessonAsync(int lessonId)
        {
            return await sectionInteractor.GetSectionsFromLessonAsync(lessonId);
        }

        [Authorize]
        [HttpGet("get/by-lesson-order")]
        public async Task<Response> GetSectionByLessonAndOrder(int lessonId, int order)
        {
            return await sectionInteractor.GetSectionByLessonAndOrderAsync(lessonId, order);
        }

        //[Authorize]
        //[HttpPost("create")]
        //public async Task<Response> CreateSectionAsync([FromForm] SectionDto sectionDto)
        //{
        //    return await sectionInteractor.CreateSectionAsync(sectionDto.LessonId, sectionDto);
        //}

        [Authorize]
        [HttpPost("create/text")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionTextContentDto sectionTextDto)
        {
            return await sectionInteractor.CreateSectionAsync(sectionTextDto.LessonId, sectionTextDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("create/file")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionFileContentDto sectionFileDto)
        {
            return await sectionInteractor.CreateSectionAsync(sectionFileDto.LessonId, sectionFileDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("create/code")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionCodeContentDto sectionCodeDto)
        {
            return await sectionInteractor.CreateSectionAsync(sectionCodeDto.LessonId, sectionCodeDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/order")]
        public async Task<Response> ChangeSectionOrderAsync(int sectionId, int lessonId, bool increase)
        {
            return await sectionInteractor.ChangeOrder(sectionId, lessonId, increase);
        }

        //[Authorize]
        //[HttpPost("update")]
        //public async Task<Response> UpdateSectionAsync([FromForm] SectionDto sectionDto, [FromQuery] int lessonId)
        //{
        //    return await sectionInteractor.UpdateSectionAsync(lessonId, sectionDto);
        //}

        [Authorize]
        [HttpPost("update/text")]
        public async Task<Response> UpdateSectionAsync([FromForm] SectionTextContentDto sectionTextDto)
        {
            return await sectionInteractor.UpdateSectionAsync(sectionTextDto.LessonId, sectionTextDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/file")]
        public async Task<Response> UpdateSectionAsync([FromForm] SectionFileContentDto sectionFileDto)
        {
            return await sectionInteractor.UpdateSectionAsync(sectionFileDto.LessonId, sectionFileDto.ToSectionDto());
        }

        [Authorize]
        [HttpPost("update/code")]
        public async Task<Response> UpdateSectionAsync([FromForm] SectionCodeContentDto sectionCodeDto)
        {
            return await sectionInteractor.UpdateSectionAsync(sectionCodeDto.LessonId, sectionCodeDto.ToSectionDto());
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveSectionAsync(int sectionId)
        {
            return await sectionInteractor.RemoveSectionAsync(sectionId);
        }
    }
}
