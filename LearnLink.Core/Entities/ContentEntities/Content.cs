namespace LearnLink.Core.Entities
{
    public class Content
    {
        public bool IsText { get; set; }
        public bool IsCodeBlock { get; set; }
        public bool IsFile { get; set; }

        public string? Text { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? CodeLanguage { get; set; }
    }
}
