using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для получения состояния авторизации
    /// </summary>
    [ApiController]
    [Route("api/Authentication")]
    public class AuthController : Controller
    {

        /// <summary>
        /// Получение никнейма авторизованного пользователя
        /// </summary>
        [Authorize]
        [HttpGet("get-auth-nickname")]
        public Response<string> GetAuthorizedNickname()
        {
            if (User.Identity == null || string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return new()
                {
                    Success = false,
                    Message = "Пользователь не авторизован",
                };
            }

            return new()
            {
                Success = true,
                Message = "Пользователь авторзиован",
                Value = User.Identity.Name,
            };
        }
    }
}
