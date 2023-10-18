using CoursesPrototype.Shared.Exceptions;
using CoursesPrototype.Application.Helpers;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.ToClientData.DataTransferObjects;

namespace CoursesPrototype.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserDto userDto)
        {
            if (!ValidateHelper.ValidateToEmptyStrings(userDto.Nickname, userDto.Name, userDto.Lastname))
            {
                throw new ForClientSideBaseException("Не все поля пользователя были заполнены");
            }

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
            return new UserDto()
            {
                Id = userEntity.Id,
                Nickname = userEntity.Nickname,
                Lastname = userEntity.Lastname,
                Name = userEntity.Name,
            };
        }
    }
}
