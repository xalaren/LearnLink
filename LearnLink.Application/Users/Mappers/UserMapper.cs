using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        (
            Id: user.Id.Value,
            Nickname: user.Nickname,
            Name: user.Name,
            Lastname: user.Lastname,
            CreatedOn: user.CreatedOnUtc,
            ModifiedOn: user.ModifiedOnUtc,
            Role: user.Role?.ToDto()
        );
    }
}
