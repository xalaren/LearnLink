using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Pages.Lessons
{
    public class LessonsActionsPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            return AuthRequired();
        }
    }
}
