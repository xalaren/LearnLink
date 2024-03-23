using LearnLink.Application.Interactors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Users.PageModels
{
    public class UsersPageModel : PageModel
    {
        private readonly UserInteractor userInteractor;

        protected UserInteractor UserInteractor => userInteractor;

        public UsersPageModel(UserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
    }
}
