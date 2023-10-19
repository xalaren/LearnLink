using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthController : Controller
    {
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
