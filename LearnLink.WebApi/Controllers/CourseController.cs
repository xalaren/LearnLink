using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления курсами
    /// </summary>
    [ApiController]
    [Route("api/Courses")]
    public class CourseController : Controller
    {
        private readonly CourseInteractor courseInteractor;
        private readonly UserVerifierService userVerifierService;

        public CourseController(CourseInteractor courseInteractor, UserVerifierService userVerifierService)
        {
            this.courseInteractor = courseInteractor;
            this.userVerifierService = userVerifierService;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            return await courseInteractor.GetCourseAsync(courseId);
        }

        [HttpGet("get-all")]
        public async Task<Response<CourseDto[]>> GetAllCoursesAsync()
        {
            return await courseInteractor.GetAllAsync();
        }

        [Authorize]
        [HttpGet("get-user-courses")]
        public async Task<Response<CourseDto[]>> GetUserCreatedCoursesAsync(int userId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success)
            {
                return new Response<CourseDto[]>()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await courseInteractor.GetCoursesCreatedByUserAsync(userId);
        }


        [AllowAnonymous]
        [HttpGet("get-public")]
        public async Task<Response<CourseDto[]>> GetPublicCoursesAsync()
        {
            return await courseInteractor.GetPublicCoursesAsync();
        }

        [Authorize]
        [HttpGet("get-subscribed")]
        public async Task<Response<CourseDto[]>> GetSubscribedCoursesAsync(int userId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<CourseDto[]>()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.GetSubscribedCoursesAsync(userId);
        }

        [Authorize]
        [HttpGet("get-creator-status")]
        public async Task<Response<bool>> IsCreatorAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<bool>()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.IsCreator(userId, courseId);
        }

        [Authorize]
        [HttpGet("get-subscriber-status")]
        public async Task<Response<bool>> IsSubscriberAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<bool>()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.IsSubscriber(userId, courseId);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.CreateCourseAsync(userId, courseDto);
        }


        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateCourseAsync(int userId, CourseDto courseDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.UpdateCourseAsync(courseDto);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveCourseAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.RemoveCourseAsync(courseId);
        }
    }
}
