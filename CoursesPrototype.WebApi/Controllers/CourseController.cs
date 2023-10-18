using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;
using CoursesPrototype.Shared.ToClientData.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/Courses")]
    public class CourseController
    {
        private readonly CourseInteractor courseInteractor;

        public CourseController(CourseInteractor courseInteractor)
        {
            this.courseInteractor = courseInteractor;
        }

        [HttpPost("create")]
        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            return await courseInteractor.CreateCourseAsync(userId, courseDto);
        }
    }
}
