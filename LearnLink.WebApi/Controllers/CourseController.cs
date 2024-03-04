using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
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

        [AllowAnonymous]
        [HttpGet("get-any")]
        public async Task<Response<CourseDto?>> GetAnyCourseAsync(int userId, int courseId)
        {
            return await courseInteractor.GetAnyCourseAsync(userId, courseId);
        }

        [HttpGet("get-all")]
        public async Task<Response<CourseDto[]>> GetAllCoursesAsync()
        {
            return await courseInteractor.GetAllAsync();
        }

        [Authorize]
        [HttpGet("get-user-courses")]
        public async Task<Response<DataPage<CourseDto[]>>> GetUserCreatedCoursesAsync(int userId, int page, int size)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success)
            {
                return new Response<DataPage<CourseDto[]>>()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await courseInteractor.GetCoursesCreatedByUserAsync(userId, new DataPageHeader(page, size));
        }


        [AllowAnonymous]
        [HttpGet("get-public")]
        public async Task<Response<DataPage<CourseDto[]>>> GetPublicCoursesAsync(int page, int size)
        {
            return await courseInteractor.GetPublicCoursesAsync(new DataPageHeader(page, size));
        }

        [Authorize]
        [HttpGet("get-subscribed")]
        public async Task<Response<DataPage<CourseDto[]>>> GetSubscribedCoursesAsync(int userId, int page, int size)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<DataPage<CourseDto[]>>()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.GetSubscribedCoursesAsync(userId, new DataPageHeader(page, size));
        }

        [Authorize]
        [HttpGet("get-unavailable")]
        public async Task<Response<DataPage<CourseDto[]>>> GetUnavailableCoursesAsync(int userId, int page, int size)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<DataPage<CourseDto[]>>()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.GetUnavailableUserCoursesAsync(userId, new DataPageHeader(page, size));
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

        [AllowAnonymous]
        [HttpGet("find-public")]
        public async Task<Response<CourseDto[]>> FindPublicCourses(string title)
        {
            return await courseInteractor.FindCoursesByTitle(title, true, false, false);
        }
    }
}
