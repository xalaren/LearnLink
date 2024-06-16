using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record AnswerDto
    {
        public int Id { get; init; }

        public int UserId { get; init; }
        public UserDto User { get; init; } = null!;
        
        public int ObjectiveId { get; init; }
        public string? Text { get; init; }
        public string? FileName { get; init; }
        public string? FileExtension { get; init; }
        public string? FileUrl { get; init; }
        public IFormFile? FormFile { get; init; }
    }
}
