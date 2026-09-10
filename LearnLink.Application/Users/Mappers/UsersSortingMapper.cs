using LearnLink.Application.Shared.Sorting;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

internal static class UsersSortingMapper
{
    public static IOrderedQueryable<User> SortBy(this IQueryable<User> baseQuery, ISortedRequest request)
    {
        return request.SortBy?.ToLowerInvariant() switch
        {
            "nickname" => request.Descending ? baseQuery.OrderByDescending(u => u.Nickname) : baseQuery.OrderBy(u => u.Nickname),
            "name" => request.Descending ? baseQuery.OrderByDescending(u => u.Name) : baseQuery.OrderBy(u => u.Name),
            "createdon" => request.Descending ? baseQuery.OrderByDescending(u => u.CreatedOnUtc) : baseQuery.OrderBy(u => u.CreatedOnUtc),
            "roleName" => request.Descending ? baseQuery.OrderByDescending(u => u.Role!.Name) : baseQuery.OrderBy(u => u.Role!.Name),
            _ => request.Descending ? baseQuery.OrderByDescending(u => u.CreatedOnUtc) : baseQuery.OrderBy(u => u.CreatedOnUtc)
        };
    }
}
