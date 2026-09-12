namespace LearnLink.Application.Users.Models;

/// <summary>
/// Data transfer object representing user information returned by application services.
/// </summary>
/// <param name="Id">User identifier.</param>
/// <param name="Nickname">User nickname.</param>
/// <param name="Name">First name.</param>
/// <param name="Lastname">Last name.</param>
/// <param name="CreatedOn">Creation timestamp (UTC).</param>
/// <param name="ModifiedOn">Last modification timestamp (UTC).</param>
/// <param name="Role">Optional role information.</param>
/// <param name="AvatarUrl">Optional avatar URL.</param>
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
