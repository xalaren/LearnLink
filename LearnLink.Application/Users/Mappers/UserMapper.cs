using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Storaging.Extensions;

namespace LearnLink.Application.Users.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        (
            Id: user.Id.Value,
            Nickname: user.Nickname,
            Name: user.Name,
            Lastname: user.Lastname,
            CreatedOn: user.CreatedOnUtc,
            ModifiedOn: user.ModifiedOnUtc,
            Role: user.Role?.ToDto(),
            AvatarUrl: user.Avatar?.ToUrl()
        );
    }
}
