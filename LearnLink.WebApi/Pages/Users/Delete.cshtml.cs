using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class DeleteModel : UsersBasePageModel
    {
        public DeleteModel(UserInteractor userInteractor) : base(userInteractor) { }

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

        public async Task OnPost(int userId)
        {
            QueryResult = await UserInteractor.RemoveUserAsync(userId);
        }
    }
}
