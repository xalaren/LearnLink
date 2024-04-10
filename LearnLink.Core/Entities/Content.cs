using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public bool IsText { get; set; }
        public bool IsCodeBlock { get; set; }
        public bool IsFile { get; set; }

        public string? Text { get; set; }
        public string? FileName { get; set; }
    }
}
