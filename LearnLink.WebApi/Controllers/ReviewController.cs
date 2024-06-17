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
    [Route("api/reviews")]
    public class ReviewController(ReviewInteractor reviewInteractor) : Controller
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<ReviewDto?>> GetReviewByAnswerAsync(int answerId)
        {
            return await reviewInteractor.GetReviewByAnswerAsync(answerId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateReviewAsync(ReviewDto reviewDto, int answerId)
        {
            return await reviewInteractor.CreateReviewAsync(reviewDto, answerId);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateReviewAsync(ReviewDto reviewDto)
        {
            return await reviewInteractor.UpdateReviewAsync(reviewDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveReviewAsync(int reviewId)
        {
            return await reviewInteractor.RemoveReviewAsync(reviewId);
        }
    }
}
