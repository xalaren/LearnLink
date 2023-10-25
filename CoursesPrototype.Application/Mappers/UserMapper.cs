using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
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
                Name = userDto.Name,
            };
        }

        public static UserDto ToDto(this User userEntity)
        {
            return new UserDto
                (
                    Id: userEntity.Id,
                    Nickname: userEntity.Nickname,
                    Lastname: userEntity.Lastname,
                    Name: userEntity.Name
                );
        }

        public static User Assign(this User user, UserDto userDto)
        {
            if(!string.Equals(user.Nickname, userDto.Nickname))
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

            return user;
        }
    }
}
