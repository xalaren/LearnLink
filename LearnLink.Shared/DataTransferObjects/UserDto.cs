namespace LearnLink.Shared.DataTransferObjects;

public record UserDto
{
    public int Id { get; init; }
    public string Nickname { get; init; } = string.Empty;
    public string Lastname { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;

    public int? RoleId { get; init; }
    public RoleDto? Role { get; init; }
}
