using LearnLink.Application.Interactors;
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
        [HttpPost("create")]
        public async Task<Response> CreateSectionAsync([FromForm] SectionDto sectionDto, [FromQuery] int lessonId)
        {
            return await sectionInteractor.CreateSectionAsync(lessonId, sectionDto);
        }
    }
}
