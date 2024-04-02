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
                RemoveAccess = roleDto.RemoveAccess,
                ManageInternalAccess = roleDto.ManageInternalAccess,
                InviteAccess = roleDto.InviteAccess,
                KickAccess = roleDto.KickAccess
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
                RemoveAccess: roleEntity.RemoveAccess,
                ManageInternalAccess: roleEntity.ManageInternalAccess,
                InviteAccess: roleEntity.InviteAccess,
                KickAccess: roleEntity.KickAccess
            );
        }

        public static LocalRole Assign(this LocalRole role, LocalRoleDto roleDto)
        {
            if (!string.Equals(role.Name, roleDto.Name) && !string.IsNullOrWhiteSpace(roleDto.Name))
            {
                role.Name = roleDto.Name;
            }

            if (!string.Equals(role.Sign, roleDto.Sign) && !string.IsNullOrWhiteSpace(roleDto.Sign))
            {
                role.Sign = roleDto.Sign;
            }

            role.ViewAccess = roleDto.ViewAccess;
            role.EditAcess = roleDto.EditAccess;
            role.RemoveAccess = roleDto.RemoveAccess;
            role.ManageInternalAccess = roleDto.ManageInternalAccess;
            role.InviteAccess = roleDto.InviteAccess;
            role.KickAccess = roleDto.KickAccess;

            return role;
        }
    }
}
