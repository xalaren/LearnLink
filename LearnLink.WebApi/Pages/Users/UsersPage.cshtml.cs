using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Users
{
    public class UserEndpointsModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            RequireAuthorize();

            if (!AdminAuthorized)
            {
                return AccessDeniedPage();
            }

            return Page();
        }
    }
}
