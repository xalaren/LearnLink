namespace LearnLink.Shared.DataTransferObjects
{
    public record LessonDto
        (
            int Id,
            string Title,
            string? Description
        );
}
