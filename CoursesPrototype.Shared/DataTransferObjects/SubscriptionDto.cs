namespace CoursesPrototype.Shared.DataTransferObjects
{
    public record SubscriptionDto(
        int UserId,
        int CourseId,
        DateTime StartDate
    );
}
