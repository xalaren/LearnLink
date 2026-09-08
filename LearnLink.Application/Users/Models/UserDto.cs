namespace LearnLink.Application.Users.Models;

public record UserDto
(
    Guid Id,
    string Nickname,
    string Name,
    string Lastname,
    DateTime CreatedOn,
    DateTime ModifiedOn,
    RoleDto? Role = null,
    string? AvatarUrl = null
);