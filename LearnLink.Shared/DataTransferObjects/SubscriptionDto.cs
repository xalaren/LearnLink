namespace LearnLink.Shared.DataTransferObjects
{
    public record SubscriptionDto(
        int UserId,
        int CourseId,
        DateTime StartDate,
        bool Completed =false,
        int CompletionProgress = 0
    );
}
