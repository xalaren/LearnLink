using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Model.Users;

namespace LearnLink.Application.Mappers.Users;

public static class RoleMapper
{
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto
        (
            Id: role.Id.Value,
            Name: role.Name,
            IsAdmin: role.IsAdmin
        );
    }
}
