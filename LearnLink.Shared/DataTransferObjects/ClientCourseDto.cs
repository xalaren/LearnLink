using LearnLink.Shared.DataTransferObjects;

public class ClientCourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description;
    public string CreationDate { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public bool IsUnavailable { get; set; }
    public int SubscribersCount { get; set; } = 0;
    public int? CompletionProgress { get; set; }
    public bool? Completed { get; set; }
    public LocalRoleDto? LocalRole { get; set; }
}