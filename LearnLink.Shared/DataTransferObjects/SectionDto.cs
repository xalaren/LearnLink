namespace LearnLink.Shared.DataTransferObjects
{
    public record SectionDto
        (
            int Id,
            int Order,
            ContentDto Content,
            int LessonId = 0,
            string? Title = null
        );
}
