using LearnLink.Domain.Entities.Users.Identifiers;

namespace LearnLink.Application.Abstractions;

public interface ICurrentUserContext
{
    UserId GetUserId();
    string GetNickname();
    RoleId GetRoleId();
}
