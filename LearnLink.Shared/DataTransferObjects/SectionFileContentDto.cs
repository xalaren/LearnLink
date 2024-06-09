using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public class SectionFileContentDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Title { get; set; }

        public IFormFile FormFile { get; set; } = null!;
    }
}
