namespace LearnLink.Shared.Model.Users;

public record RoleDto
(
    Guid Id,
    string Name,
    bool IsAdmin,
    bool IsSuper
);
