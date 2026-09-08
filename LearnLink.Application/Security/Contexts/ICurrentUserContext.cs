using LearnLink.Domain.Entities.Users.Identifiers;

namespace LearnLink.Application.Security.Contexts;

public interface ICurrentUserContext
{
    UserId GetUserId();
    string GetNickname();
    RoleId GetRoleId();
}
