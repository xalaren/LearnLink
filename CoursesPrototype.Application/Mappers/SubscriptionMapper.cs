using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
{
    public static class SubscriptionMapper
    {
        public static Subscription ToEntity(this SubscriptionDto subscriptionDto)
        {
            return new Subscription()
            {
                Id = subscriptionDto.Id,
                StartDate = subscriptionDto.StartDate,
                EndDate = subscriptionDto.EndDate,
                CourseId = subscriptionDto.CourseId,
                UserId = subscriptionDto.UserId,
            };
        }

        public static SubscriptionDto ToDto(this Subscription subscriptionEntity)
        {
            return new SubscriptionDto()
            {
                Id = subscriptionEntity.Id,
                StartDate = subscriptionEntity.StartDate,
                EndDate = subscriptionEntity.EndDate,
                CourseId = subscriptionEntity.CourseId,
                UserId = subscriptionEntity.UserId,
            };
        }
    }
}
