using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public class ContentDto
    {
        public bool IsText { get; set; }
        public bool IsCodeBlock { get; set; }
        public bool IsFile { get; set; }

        public string? Text { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? FileUrl { get; set; }
        public IFormFile? FormFile { get; set; }
        public string? CodeLanguage { get; set; }
    }
}
