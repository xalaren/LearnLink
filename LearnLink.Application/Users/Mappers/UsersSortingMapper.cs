using System.Linq.Expressions;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

internal static class UsersSortingMapper
{
    private static readonly Dictionary<string, Expression<Func<User, object>>> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["nickname"] = user => user.Nickname,
        ["name"] = user => user.Name,
        ["lastname"] = user => user.Lastname,
        ["createdOn"] = user => user.CreatedOnUtc,
        ["modifiedOn"] = user => user.ModifiedOnUtc,
        ["roleId"] = user => user.Role.Id,
        ["roleName"] = user => user.Role.Name
    };


    public static Expression<Func<User, object>> Resolve(string? sortBy)
    {
        if (sortBy != null && Map.TryGetValue(sortBy, out var expression))
        {
            return expression;
        }

        return user => user.CreatedOnUtc;
    }
}
