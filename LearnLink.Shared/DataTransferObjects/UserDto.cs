using Microsoft.AspNetCore.Http;

namespace LearnLink.Shared.DataTransferObjects;

public record UserDto
{
    public int Id { get; init; }
    public string Nickname { get; init; } = string.Empty;
    public string Lastname { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public RoleDto? Role { get; init; }
    public IFormFile? AvatarFormFile { get; set; }
    public string? AvatarFileName { get; set; }
    public string? AvatarUrl { get; set; }
}
