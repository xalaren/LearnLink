using LearnLink.Application.Interactors;
namespace LearnLink.WebApi.Pages.PageModels
{
    public class SubscriptionsBasePageModel : AuthorizePageModel
    {
        private readonly SubscriptionInteractor subscriptionInteractor;

        protected SubscriptionInteractor SubscriptionInteractor => subscriptionInteractor;

        public SubscriptionsBasePageModel(SubscriptionInteractor subscriptionInteractor)
        {
            this.subscriptionInteractor = subscriptionInteractor;
        }
    }
}
