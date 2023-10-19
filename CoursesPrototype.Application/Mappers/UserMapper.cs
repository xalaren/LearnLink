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
            user.Nickname = userDto.Nickname;
            user.Name = userDto.Name;
            user.Lastname = userDto.Lastname;

            return user;
        }
    }
}
