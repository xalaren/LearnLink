namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string Title,
          string? Description,
          bool IsPublic = false,
          bool IsUnavailable = false,
          int SubscribersCount = 0
        );
}
