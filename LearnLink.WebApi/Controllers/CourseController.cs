using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
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

        [AllowAnonymous]
        [HttpGet("get/any")]
        public async Task<Response<ClientCourseDto?>> GetAnyCourseAsync(int userId, int courseId)
        {
            return await courseInteractor.GetAnyCourseAsync(userId, courseId);
        }

        [HttpGet("get/all")]
        public async Task<Response<CourseDto[]>> GetAllCoursesAsync()
        {
            return await courseInteractor.GetAllAsync();
        }

        [Authorize]
        [HttpGet("get/user-courses")]
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
        public async Task<Response<DataPage<CourseDto[]>>> GetPublicCoursesAsync(int page, int size)
        {
            return await courseInteractor.GetPublicCoursesAsync();
        }

        [Authorize]
        [HttpGet("get-subscribed")]
        public async Task<Response<DataPage<CourseDto[]>>> GetSubscribedCoursesAsync(int userId, int page, int size)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new Response<CourseDto[]>()
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
        [HttpGet("get/status/creator")]
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
        [HttpGet("get/status/subscriber")]
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
        [HttpGet("find/participants")]
        public async Task<Response<DataPage<ParticipantDto[]>>> FindParticipantsAsync(int userId, int courseId, string? searchText, int page, int size)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await courseInteractor.FindParticipantsAsync(userId, courseId, searchText, new DataPageHeader(page, size));
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

            return await courseInteractor.UpdateCourseAsync(userId, courseDto);
        }

        [Authorize]
        [HttpPost("setUnavailable")]
        public async Task<Response> SetCourseUnavailableAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.SetCourseUnavailableAsync(userId, courseId);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveCourseAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.RemoveCourseAsync(userId, courseId);
        }
    }
}