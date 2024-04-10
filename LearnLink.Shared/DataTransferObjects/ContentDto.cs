using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record ContentDto
        (
            int Id,
            bool IsText,
            bool IsCodeBlock,
            bool IsFile,
            string? Text = "",
            string? FileName = "",
            string? FileUrl = "",
            IFormFile? FormFile = null
        );
}
