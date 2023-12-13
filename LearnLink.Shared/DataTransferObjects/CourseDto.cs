namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string Title,
          string? Description,
          bool IsPublic = false,
          int SubscribersCount = 0
        );
}
