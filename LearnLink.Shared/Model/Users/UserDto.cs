namespace LearnLink.Shared.Model.Users;

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