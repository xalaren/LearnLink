namespace LearnLink.Application.Users.Models;

/// <summary>
/// Data transfer object representing a role exposed by application services.
/// </summary>
/// <param name="Id">Role identifier.</param>
/// <param name="Name">Role name.</param>
/// <param name="IsAdmin">Whether the role has administrative privileges.</param>
/// <param name="IsSuper">Whether the role is a super/system role.</param>
public record RoleDto
(
    Guid Id,
    string Name,
    bool IsAdmin,
    bool IsSuper
);
