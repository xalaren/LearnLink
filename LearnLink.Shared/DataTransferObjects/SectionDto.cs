namespace LearnLink.Shared.DataTransferObjects
{
    public record SectionDto
        (
            int LessonId,
            int ContentId,
            ContentDto Content,
            string Title,
            int Order = 0
        );
}
