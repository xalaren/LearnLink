namespace LearnLink.Application.Users.Models;

public record RoleDto
(
    Guid Id,
    string Name,
    bool IsAdmin,
    bool IsSuper
);
