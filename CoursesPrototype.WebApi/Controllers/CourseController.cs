using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
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

        //[Authorize]
        [HttpGet("get")]
        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            return await courseInteractor.GetCourseAsync(courseId);
        }

        //TODO: make private
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

        //[Authorize]
        [HttpGet("get-subscribed")]
        public async Task<Response<CourseDto[]>> GetSubscribedCoursesAsync(int userId)
        {
            //var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            //if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.GetSubscribedCoursesAsync(userId);
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            //var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            //if (!verifyResponse.Success) return verifyResponse;

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
