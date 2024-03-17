using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class RoleMapper
    {
        public static Role ToEntity(this RoleDto roleDto)
        {
            return new Role()
            {
                Id = roleDto.Id,
                Name = roleDto.Name,
                Sign = roleDto.Sign,
                IsAdmin = roleDto.IsAdmin
            };
        }

        public static RoleDto ToDto(this Role roleEntity)
        {
            return new RoleDto(
                Id: roleEntity.Id,
                Name: roleEntity.Name,
                Sign: roleEntity.Sign,
                IsAdmin: roleEntity.IsAdmin
            );
        }

        public static Role Assign(this Role role, RoleDto roleDto)
        {
            if (!string.Equals(role.Name, roleDto.Name))
            {
                role.Name = roleDto.Name;
            }

            if (!string.Equals(role.Sign, roleDto.Sign))
            {
                role.Sign = roleDto.Sign;
            }

            role.IsAdmin = roleDto.IsAdmin;

            return role;
        }
    }
}
