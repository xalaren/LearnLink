namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string? Description,
          string Title,
          bool IsPublic = false,
          int CreatorsCount = 0,
          int SubscribersCount = 0
        );
}
