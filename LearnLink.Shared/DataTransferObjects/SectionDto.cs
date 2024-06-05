namespace LearnLink.Shared.DataTransferObjects
{
    public class SectionDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public ContentDto? Content { get; set; }
        public int LessonId { get; set; }
        public string? Title { get; set; }
    }
}
