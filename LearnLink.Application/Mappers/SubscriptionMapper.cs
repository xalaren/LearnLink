using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class SubscriptionMapper
    {
        public static Subscription ToEntity(this SubscriptionDto subscriptionDto)
        {
            return new Subscription()
            {
                StartDate = subscriptionDto.StartDate.ToUniversalTime(),
                CourseId = subscriptionDto.CourseId,
                UserId = subscriptionDto.UserId,
                Completed = subscriptionDto.Completed,
                CompletionProgress = subscriptionDto.CompletionProgress,
            };
        }

        public static SubscriptionDto ToDto(this Subscription subscriptionEntity)
        {
            return new SubscriptionDto
            (
                StartDate: subscriptionEntity.StartDate,
                CourseId: subscriptionEntity.CourseId,
                UserId: subscriptionEntity.UserId,
                Completed: subscriptionEntity.Completed,
                CompletionProgress: subscriptionEntity.CompletionProgress
            );
        }
    }
}
