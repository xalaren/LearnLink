using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Modules
{
    public class ModulesActionsPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            return AuthRequired();
        }
    }
}
