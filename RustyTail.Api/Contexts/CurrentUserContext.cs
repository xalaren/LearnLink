using System.Security.Claims;
using RustyTail.Application.Security.Contexts;
using RustyTail.Domain.Entities.Users.Identifiers;

namespace RustyTail.Api.Contexts;

public sealed class CurrentUserContext(IHttpContextAccessor contextAccessor) : ICurrentUserContext
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string? GetNickname()
    {
        return _contextAccessor.HttpContext?.User.Identity?.Name;
    }

    public RoleId? GetRoleId()
    {
        string? roleIdentifier = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        bool isParsed = Guid.TryParse(roleIdentifier, out Guid roleId);

        if (!isParsed) return null;

        return new RoleId(roleId);
    }

    public UserId? GetUserId()
    {
        string? nameIdentifier = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isParsed = Guid.TryParse(nameIdentifier, out Guid userId);

        if (!isParsed) return null;

        return new UserId(userId);
    }
}
