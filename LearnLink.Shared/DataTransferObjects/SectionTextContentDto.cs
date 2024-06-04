namespace LearnLink.Shared.DataTransferObjects
{
    public class SectionTextContentDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int LessonId { get; set; }
        public string? Title { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
