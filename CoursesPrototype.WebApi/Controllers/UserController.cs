using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePrototype.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// </summary>
    [ApiController]
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly UserInteractor userInteractor;
        private readonly UserVerifierService userVerifierService;

        public UserController(UserInteractor userInteractor, UserVerifierService verifierService)
        {
            this.userInteractor = userInteractor;
            this.userVerifierService = verifierService;

        }

        /// <summary>
        /// Получение пользователя по данным авторизации
        /// </summary>
        [Authorize]
        [HttpGet("get")]
        public async Task<Response<UserDto>> GetUserAsync()
        {
            var nickname = User.Identity?.Name;

            return await userInteractor.GetUserByNicknameAsync(nickname);
        }

        /// <summary>
        /// Регистрация пользователя в системе
        /// </summary>
        /// <param name="userDto">Объект данных пользователя</param>
        /// <param name="password">Пароль для входа</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(UserDto userDto, string password)
        {
            return await userInteractor.RegisterAsync(userDto, password);
        }
        
        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <param name="nickname">Никнейм пользователя</param>
        /// <param name="password">Пароль для входа</param>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Response<string>> Login(string nickname, string password)
        {
            return await userInteractor.AuthenticateAsync(nickname, password);
        }

        /// <summary>
        /// Редактирование данных пользователя в системе
        /// </summary>
        /// <param name="userDto">Объект данных пользователя</param>
        [Authorize]
        [HttpPost("update-user")]
        public async Task<Response<string?>> UpdateUserAsync(UserDto userDto)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userDto.Id);

            if (!verifyResponse.Success)
            {
                return new()
                {
                    Success = verifyResponse.Success,
                    Message = verifyResponse.Message,
                };
            }

            return await userInteractor.UpdateUserAsync(userDto);
        }

        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый парольь</param>
        [Authorize]
        [HttpPost("update-pass")]
        public async Task<Response> UpdatePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await userInteractor.UpdateUserPasswordAsync(userId, oldPassword, newPassword);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [Authorize]
        [HttpDelete("remove")]
        public async Task<Response> RemoveAccountAsync(int userId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return verifyResponse;

            return await userInteractor.RemoveUserAsync(userId);
        }
    }
}
