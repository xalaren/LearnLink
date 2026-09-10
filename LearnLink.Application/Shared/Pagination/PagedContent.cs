using LearnLink.Application.Shared.Pagination.Abstractions;

namespace LearnLink.Application.Shared.Pagination;
public record PagedContent<TContent>
(
    int Page,
    int PerPage,
    int Count,
    IReadOnlyCollection<TContent> Items
) : IPagedContent<TContent>
{   
    public int Pages => (int)Math.Ceiling(Count / (double)PerPage);
    public bool HasNextPage => Page < Pages;
    public bool HasPreviousPage => Page > 1;
}
