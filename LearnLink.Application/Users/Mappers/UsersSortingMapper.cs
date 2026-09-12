using LearnLink.Application.Shared.Sorting;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Mappers;

/// <summary>
/// Helpers to apply sorting to user queries based on <see cref="ISortedRequest"/>.
/// </summary>
internal static class UsersSortingMapper
{
    /// <summary>
    /// Applies sorting to the provided user query according to the request.
    /// </summary>
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
