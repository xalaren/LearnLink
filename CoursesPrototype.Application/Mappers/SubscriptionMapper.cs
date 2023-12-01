﻿using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
{
    public static class SubscriptionMapper
    {
        public static Subscription ToEntity(this SubscriptionDto subscriptionDto)
        {
            return new Subscription()
            {
                StartDate = subscriptionDto.StartDate,
                CourseId = subscriptionDto.CourseId,
                UserId = subscriptionDto.UserId,
            };
        }

        public static SubscriptionDto ToDto(this Subscription subscriptionEntity)
        {
            return new SubscriptionDto
            (
                StartDate: subscriptionEntity.StartDate,
                CourseId: subscriptionEntity.CourseId,
                UserId: subscriptionEntity.UserId
            );
        }
    }
}
