using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Subscriptions
{
    public class SubscribeModel : SubscriptionsBasePageModel
    {
        public SubscribeModel(SubscriptionInteractor subscriptionInteractor) : base(subscriptionInteractor) { }

        public Response? QueryResult { get; set; }

        public IActionResult OnGet()
        {
            return AuthRequired();
        }

        public async Task OnPost(int userId, int courseId) 
        {
            var subscription = new SubscriptionDto(userId, courseId, DateTime.Now);

            QueryResult = await SubscriptionInteractor.SubscribeAsync(subscription);
        }
    }
}
