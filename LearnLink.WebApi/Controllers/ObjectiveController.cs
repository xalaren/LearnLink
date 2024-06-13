using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/objectives")]
    public class ObjectiveController(ObjectiveInteractor objectiveInteractor)
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<ObjectiveDto?>> GetObjectiveAsync(int lessonId, int objectiveId)
        {
            return await objectiveInteractor.GetObjectiveAsync(lessonId, objectiveId);
        }

        [Authorize]
        [HttpGet("get/fromlesson")]
        public async Task<Response<ObjectiveDto[]>> GetObjectivesFromLessonAsync(int lessonId)
        {
            return await objectiveInteractor.GetObjectivesFromLessonAsync(lessonId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateObjectiveAsync([FromForm] ObjectiveDto objectiveDto, [FromQuery]int lessonId)
        {
            return await objectiveInteractor.CreateObjectiveAsync(lessonId, objectiveDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateObjectiveAsync([FromForm] ObjectiveDto objectiveDto, [FromQuery] bool removeFileContent)
        {
            return await objectiveInteractor.UpdateObjectiveAsync(objectiveDto, removeFileContent);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<Response> RemoveObjectiveAsync(int objectiveId)
        {
            return await objectiveInteractor.RemoveObjectiveAsync(objectiveId);
        }
    }
}
