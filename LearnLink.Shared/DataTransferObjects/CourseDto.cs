namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string Title,
          DateTime? CreationDate,
          string? Description,
          int SubscribersCount = 0,
          bool IsPublic = false,
          bool IsUnavailable = false,
          int? CompletionProgress = null,
          bool? Competed = null
        );
}
