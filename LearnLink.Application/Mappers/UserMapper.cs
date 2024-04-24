using LearnLink.Application.Helpers;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;

namespace LearnLink.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserDto userDto)
        {
            return new User()
            {
                Id = userDto.Id,
                Nickname = userDto.Nickname,
                Lastname = userDto.Lastname,
                Name = userDto.Name
            };
        }

        public static UserDto ToDto(this User userEntity)
        {
            return new UserDto()
            {
                Id = userEntity.Id,
                Nickname = userEntity.Nickname,
                Lastname = userEntity.Lastname,
                Name = userEntity.Name,
                Role = userEntity.Role?.ToDto(),
                AvatarFileName = userEntity.AvatarFileName,
                AvatarUrl = userEntity.AvatarFileName != null ?
                    DirectoryStore.GetRelativeDirectoryUrlToUserImages(userEntity.Id) + userEntity.AvatarFileName : null
            };
        }

        public static User Assign(this User user, UserDto userDto)
        {
            if (!string.Equals(user.Nickname, userDto.Nickname))
            {
                user.Nickname = userDto.Nickname;
            }

            if (!string.Equals(user.Name, userDto.Name))
            {
                user.Name = userDto.Name;
            }

            if (!string.Equals(user.Lastname, userDto.Lastname))
            {
                user.Lastname = userDto.Lastname;
            }

            if (!string.IsNullOrWhiteSpace(userDto.AvatarFileName))
            {
                user.AvatarFileName = userDto.AvatarFileName;
            }


            return user;
        }
    }
}
