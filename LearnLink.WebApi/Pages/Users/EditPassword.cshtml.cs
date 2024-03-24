using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users
{
    public class EditPasswordModel : UsersBasePageModel
    {
        public EditPasswordModel(UserInteractor userInteractor) : base(userInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            RequireAuthorize();

            if (!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            return Page();
        }

        public async Task OnPost(int userId, string oldPassword, string newPassword)
        {
            QueryResult = await UserInteractor.UpdateUserPasswordAsync(userId, oldPassword, newPassword);
        }
    }
}
