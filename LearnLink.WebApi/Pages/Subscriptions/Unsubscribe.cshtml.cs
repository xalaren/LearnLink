using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Subscriptions
{
    public class UnsubscribeModel : SubscriptionsBasePageModel
    {
        public UnsubscribeModel(SubscriptionInteractor subscriptionInteractor) : base(subscriptionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int userId, int courseId)
        {
            QueryResult = await SubscriptionInteractor.Unsubscribe(userId, courseId);
        }


    }
}
