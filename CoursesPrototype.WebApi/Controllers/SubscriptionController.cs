using CoursesPrototype.Application.Interactors;
using CoursesPrototype.Shared.DataTransferObjects;
using CoursesPrototype.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesPrototype.WebApi.Controllers
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

        /// <summary>
        /// Подписка на курс
        /// </summary>
        /// <param name="subscriptionDto">Объект данных подписки</param>
        [HttpPost("subscribe")]
        public async Task<Response> SubscribeAsync(SubscriptionDto subscriptionDto)
        {
            return await subscriptionInteractor.CreateSubscriptionAsync(subscriptionDto);
        }

        /// <summary>
        /// Отписка от курса
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="courseId">Идентификатор курса</param>
        [HttpDelete("unsubscribe")]
        public async Task<Response> UnsubscribeAsync(int userId, int courseId)
        {
            return await subscriptionInteractor.Unsubscribe(userId, courseId);
        }
    }
}
