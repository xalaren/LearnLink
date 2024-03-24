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
    }
}
