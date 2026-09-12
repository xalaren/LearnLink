using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

/// <summary>
/// Mapping helpers for <see cref="Role"/> to application DTOs.
/// </summary>
public static class RoleMapper
{
    /// <summary>
    /// Maps a domain <see cref="Role"/> to a <see cref="RoleDto"/>.
    /// </summary>
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto
        (
            Id: role.Id.Value,
            Name: role.Name,
            IsAdmin: role.IsAdmin,
            IsSuper: role is { IsAdmin: true, IsSystem: true }
        );
    }
}
