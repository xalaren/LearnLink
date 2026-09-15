using RustyTail.Domain.Entities.Users.Identifiers;

namespace RustyTail.Application.Security.Contexts;

/// <summary>
/// Provides information about the current authenticated user.
/// Implementations are expected to expose identifiers and basic claims
/// related to the current user context.
/// </summary>
public interface ICurrentUserContext
{
    /// <summary>
    /// Gets the identifier of the current user.
    /// </summary>
    UserId? GetUserId();

    /// <summary>
    /// Gets the nickname of the current user.
    /// </summary>
    string? GetNickname();

    /// <summary>
    /// Gets the role identifier of the current user.
    /// </summary>
    RoleId? GetRoleId();
}
