using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record FileUpload
    {
        public string Name { get; init; } = null!;
        public string Extension { get; init; } = null!;
        public string Url { get; init; } = null!;
    }
}
