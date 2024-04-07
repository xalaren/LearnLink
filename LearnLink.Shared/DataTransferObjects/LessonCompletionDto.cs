namespace LearnLink.Shared.DataTransferObjects
{
    public record LessonCompletionDto
        (
            int UserId,
            LessonDto LessonDto,
            bool Completed,
            int CompletionProgress
        );
}
