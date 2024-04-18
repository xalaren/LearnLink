namespace LearnLink.Shared.DataTransferObjects
{
    public record SectionDto
        (
            int Id,
            int Order,
            ContentDto ContentDto,
            int LessonId = 0,
            string? Title = null
        );
}
