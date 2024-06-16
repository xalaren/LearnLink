using LearnLink.Adapter.EFTransaction;
using LearnLink.Application.Interactors;
using LearnLink.Core.Entities;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/answers")]
    public class AnswerController(AnswerInteractor answerInteractor) : Controller
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<AnswerDto>> RequestGetAnswerAsync(int requesterUserId, int courseId, int lessonId, int answerId)
        {
            return await answerInteractor.RequestGetAnswerAsync(requesterUserId, courseId, lessonId, answerId);
        }

        [Authorize]
        [HttpGet("get/fromobjective")]
        public async Task<Response<DataPage<AnswerDto[]>>> RequestGetObjectiveAnswers(
            int requesterUserId,
            int courseId,
            int lessonId,
            int objectiveId,
            int page,
            int size)
        {
            return await answerInteractor.RequestGetObjectiveAnswers(
                requesterUserId,
                courseId,
                lessonId,
                objectiveId,
                new DataPageHeader(page, size));
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateAnswerAsync(int lessonId, AnswerDto answerDto)
        {
            return await answerInteractor.CreateAnswerAsync(lessonId, answerDto);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateAnswerAsync(int lessonId, AnswerDto answerDto, bool removeFileContent)
        {
            return await answerInteractor.UpdateAnswerAsync(lessonId, answerDto, removeFileContent);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveAnswerAsync(int lessonId, int answerId)
        {
            return await answerInteractor.RemoveAnswerAsync(lessonId, answerId);
        }
    }
}
