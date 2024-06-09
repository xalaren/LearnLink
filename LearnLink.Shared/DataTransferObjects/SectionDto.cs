namespace LearnLink.Shared.DataTransferObjects
{
    public class SectionDto
    {
        public int Id { get; init; }
        public int Order { get; init; }
        public ContentDto? Content { get; init; }
        public string? Title { get; init; }
    }
}
