using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

/// <summary>
/// Mapping helpers for <see cref="User"/> domain entities to application DTOs.
/// </summary>
public static class UserMapper
{
    /// <summary>
    /// Maps a domain <see cref="User"/> to a <see cref="UserDto"/>.
    /// </summary>
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
            Role: user.Role?.ToDto()
        );
    }
}
