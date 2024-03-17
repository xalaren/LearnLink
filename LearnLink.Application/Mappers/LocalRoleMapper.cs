using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class LocalRoleMapper
    {
        public static LocalRole ToEntity(this LocalRoleDto roleDto)
        {
            return new LocalRole()
            {
                Id = roleDto.Id,
                Name = roleDto.Name,
                Sign = roleDto.Sign,
                ViewAccess = roleDto.ViewAccess,
                EditAcess = roleDto.EditAccess,
                RemoveAcess = roleDto.RemoveAccess,
                ManageInternalAccess = roleDto.ManageInternalAccess,
            };
        }

        public static LocalRoleDto ToDto(this LocalRole roleEntity)
        {
            return new LocalRoleDto(
                Id: roleEntity.Id,
                Name: roleEntity.Name,
                Sign: roleEntity.Sign,
                ViewAccess: roleEntity.ViewAccess,
                EditAccess: roleEntity.EditAcess,
                RemoveAccess: roleEntity.RemoveAcess,
                ManageInternalAccess: roleEntity.ManageInternalAccess
            );
        }

        public static LocalRole Assign(this LocalRole role, LocalRoleDto roleDto)
        {
            if (!string.Equals(role.Name, roleDto.Name))
            {
                role.Name = roleDto.Name;
            }

            if (!string.Equals(role.Sign, roleDto.Sign))
            {
                role.Sign = roleDto.Sign;
            }

            role.ViewAccess = roleDto.ViewAccess;
            role.EditAcess = roleDto.EditAccess;
            role.RemoveAcess = roleDto.RemoveAccess;
            role.ManageInternalAccess = roleDto.ManageInternalAccess;

            return role;
        }
    }
}
