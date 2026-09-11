using LearnLink.Application.Shared.Pagination;
using LearnLink.Application.Users.Models;

namespace LearnLink.Application.Users.Queries
{
    public record ListQueryResult(int Page, int PerPage, int Count, IReadOnlyCollection<UserDto> Items) : PagedContent<UserDto>(Page, PerPage, Count, Items);
}
