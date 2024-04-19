using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Completions
{
    public class CompletionsActionsPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            return AuthRequired();
        }
    }
}
