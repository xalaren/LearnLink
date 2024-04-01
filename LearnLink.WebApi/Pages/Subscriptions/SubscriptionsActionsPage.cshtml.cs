using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Subscriptions
{
    public class SubscriptionsActionsPageModel : AuthorizePageModel
    {
        public IActionResult OnGet()
        {
            return AuthRequired();
        }
    }
}
