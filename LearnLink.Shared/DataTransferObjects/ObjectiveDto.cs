using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LearnLink.Shared.DataTransferObjects
{
    public class ObjectiveDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public IFormFile? FormFile { get; set; }
        public string? FileUrl { get; set; }
    }
}
