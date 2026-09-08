using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

public static class RoleMapper
{
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
