namespace LearnLink.Shared.DataTransferObjects
{
    public record CourseDto
        (
          int Id,
          string? Description,
          string Title,
          bool IsPublic = false
        );
}
