namespace CoursesPrototype.Shared.DataTransferObjects
{
    public record SubscriptionDto(
        int Id,
        int UserId,
        int CourseId,
        DateTime StartDate
    );
}
