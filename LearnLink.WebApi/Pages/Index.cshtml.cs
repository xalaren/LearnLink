using System.Security.Claims;
using LearnLink.Application.Interactors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserInteractor userInteractor;
        public string? ErrorText { get; set; }
        public string? SuccessText { get; set; }

        public IndexModel(ILogger<IndexModel> logger, UserInteractor userInteractor)
        {
            _logger = logger;
            this.userInteractor = userInteractor;
        }

        public async Task OnPost(string login, string password)
        {
            var result = await userInteractor.AuthenticateAsync(login, password, true);

            if (!result.Success)
            {
                ErrorText = result.Message;
                return;
            }

            var response = await userInteractor.GetUserByNicknameAsync(login);

            if(!response.Success)
            {
                ErrorText = result.Message;
                return;
            }

            var userRole = response.Value!.Role!;

            if(!userRole.IsAdmin)
            {
                ErrorText = "Вы не являетесь администратором";
            }

            SuccessText = result.Message;
            HttpContext.Session.SetString("IsAdmin", userRole.IsAdmin.ToString());
            //HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, login)], "Cookies"));
            //HttpContext.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, userRole.Sign) }));
        }
    }
}
