using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Subscriptions
{
    public class KickModel : SubscriptionsBasePageModel
    {
        public KickModel(SubscriptionInteractor subscriptionInteractor) : base(subscriptionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int requesterUserId, int targetUserId, int courseId)
        {
            QueryResult = await SubscriptionInteractor.KickUserAsync(requesterUserId, targetUserId, courseId);
        }
    }
}
