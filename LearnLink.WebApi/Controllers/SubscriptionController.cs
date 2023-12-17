using LearnLink.Application.Interactors;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    /// <summary>
    /// Управление подписками пользователя
    /// </summary>
    [ApiController]
    [Route("api/Subscriptions")]
    public class SubscriptionController
    {
        private readonly SubscriptionInteractor subscriptionInteractor;

        public SubscriptionController(SubscriptionInteractor subscriptionInteractor)
        {
            this.subscriptionInteractor = subscriptionInteractor;
        }

        [HttpPost("subscribe")]
        public async Task<Response> SubscribeAsync(SubscriptionDto subscriptionDto)
        {
            return await subscriptionInteractor.CreateSubscriptionAsync(subscriptionDto);
        }

        [HttpDelete("unsubscribe")]
        public async Task<Response> UnsubscribeAsync(int userId, int courseId)
        {
            return await subscriptionInteractor.Unsubscribe(userId, courseId);
        }
    }
}
