using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects
{
    public record ContentDto
        (
            bool IsText,
            bool IsCodeBlock,
            bool IsFile,
            string? Text = "",
            string? FileName = "",
            IFormFile? FormFile = null,
            string? FileUrl = ""
        );
}
