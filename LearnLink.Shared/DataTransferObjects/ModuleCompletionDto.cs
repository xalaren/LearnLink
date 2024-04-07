
namespace LearnLink.Shared.DataTransferObjects
{
    public record ModuleCompletionDto
        (
            int UserId,
            ModuleDto ModuleDto,
            bool Completed,
            int CompletionProgress
        );
}
