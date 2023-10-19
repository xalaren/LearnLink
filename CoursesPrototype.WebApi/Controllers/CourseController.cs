using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
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

            return await courseInteractor.GetUserCreatedCoursesAsync(userId);
        }

        [HttpPost("create")]
        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.CreateCourseAsync(userId, courseDto);
        }
    }
}
