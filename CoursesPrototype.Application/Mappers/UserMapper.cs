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
            if (userDto.Id == null || !ValidateHelper.ValidateToEmptyStrings(userDto.Nickname, userDto.Name, userDto.Lastname))
            {
                throw new ForClientSideBaseException("Не все поля пользователя были заполнены");
            }

            return new User()
            {
                Id = userDto.Id.Value,
                Nickname = userDto.Nickname!,
                Lastname = userDto.Lastname!,
                Name = userDto.Name!,
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Lastname = user.Lastname,
                Name = user.Name,
            };
        }
    }
}
