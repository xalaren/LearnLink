using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class LoginModel : UsersPageModel
    {
        public LoginModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response<string>? QueryResult { get; set; } 

        public IActionResult OnGet()
        {
            RequireAuthorize();

            if (!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            return Page();
        }

        public async Task OnPost(string nickname, string password)
        {
            QueryResult = await UserInteractor.AuthenticateAsync(nickname, password);
        }
    }
}
