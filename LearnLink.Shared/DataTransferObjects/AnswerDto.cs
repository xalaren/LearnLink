using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record AnswerDto
    {
        public int Id { get; init; }

        public int ObjectiveId { get; init; }

        public string UploadDate { get; init; } = string.Empty;

        public int UserId { get; init; }
        public UserLiteDetailsDto? UserDetails { get; init; }

        public string? Text { get; init; }

        public FileUpload? FileDetails { get; init; }
        public IFormFile? FormFile { get; init; }

        public int? Grade { get; init; }
    }
}
