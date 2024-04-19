using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.LocalRoles
{
    public class LocalRolesActionsPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            return AuthRequired();
        }
    }
}
