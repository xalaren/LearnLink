using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Roles
{
    public class RolesPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            RequireAuthorize();
            if(!AdminAuthorized) return AccessDeniedPage();

            return Page();
        }
    }
}
