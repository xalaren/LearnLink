using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
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

        /// <summary>
        /// Получение курса по идентификатору
        /// </summary>
        /// <param name="courseId">Идентификатор курса</param>
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<CourseDto?>> GetCourseAsync(int courseId)
        {
            return await courseInteractor.GetCourseAsync(courseId);
        }

        /// <summary>
        /// Получение всех курсов в системе
        /// </summary>
        [HttpGet("get-all")]
        public async Task<Response<CourseDto[]>> GetAllCoursesAsync()
        {
            return await courseInteractor.GetAllAsync();
        }

        /// <summary>
        /// Получение курсов, созданных пользователем
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
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


        /// <summary>
        /// Получение всех общедоступных курсов
        /// </summary>
        [AllowAnonymous]
        [HttpGet("get-public")]
        public async Task<Response<CourseDto[]>> GetPublicCoursesAsync()
        {
            return await courseInteractor.GetPublicCoursesAsync();
        }

        /// <summary>
        /// Получение курсов, на которые подписан пользователь
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
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

        /// <summary>
        /// Создание курса
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="courseDto">Объект курса</param>
        [Authorize]
        [HttpPost("create")]
        public async Task<Response> CreateCourseAsync(int userId, CourseDto courseDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.CreateCourseAsync(userId, courseDto);
        }

        /// <summary>
        /// Изменение данных курса
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="courseDto">Идентификатор курса</param>
        [Authorize]
        [HttpPost("update")]
        public async Task<Response> UpdateCourseAsync(int userId, CourseDto courseDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await courseInteractor.UpdateCourseAsync(courseDto);
        }

        /// <summary>
        /// Удаление курса
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="courseId">Идентификатор курса</param>
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
