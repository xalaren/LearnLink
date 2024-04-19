using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.PageModels
{
    public class AuthorizePageModel : PageModel
    {
        private bool adminAuthorized = false;
        protected bool AdminAuthorized => adminAuthorized;

        public void RequireAuthorize()
        {
            bool.TryParse(HttpContext.Session.GetString("IsAdmin"), out adminAuthorized);
        }

        public IActionResult AccessDeniedPage()
        {
            return RedirectToPage("/AccessDenied");
        }

        public IActionResult AuthRequired(Action? action = null)
        {
            RequireAuthorize();
            if (!AdminAuthorized) return AccessDeniedPage();

            action?.Invoke();

            return Page();
        }

        public async Task<IActionResult> AuthRequiredAsync(Func<Task> action)
        {
            RequireAuthorize();
            if (!AdminAuthorized) return AccessDeniedPage();

            if (action != null)
            {
                await action();
            }

            return Page();
        }
    }
}
