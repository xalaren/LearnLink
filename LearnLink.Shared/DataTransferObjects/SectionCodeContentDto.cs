namespace LearnLink.Shared.DataTransferObjects
{
    public class SectionCodeContentDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Title { get; set; }
        public string Code { get; set; } = string.Empty;
        public string CodeLanguage { get; set; } = string.Empty;
    }
}
