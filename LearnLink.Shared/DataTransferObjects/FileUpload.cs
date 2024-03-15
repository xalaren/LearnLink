using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record FileUpload
    {
        public byte[]? FileBytes { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
