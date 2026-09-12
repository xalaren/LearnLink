using LearnLink.Application.Shared.Pagination;
using LearnLink.Application.Users.Models;

namespace LearnLink.Application.Users.Queries;

/// <summary>
/// Result returned by <see cref="ListQueryHandler"/> containing paged users.
/// </summary>
/// <param name="Page">Current page number.</param>
/// <param name="PerPage">Items per page.</param>
/// <param name="Count">Total number of items.</param>
/// <param name="Items">The items in the current page.</param>
public record ListQueryResult(int Page, int PerPage, int Count, IReadOnlyCollection<UserDto> Items) : PagedContent<UserDto>(Page, PerPage, Count, Items);
