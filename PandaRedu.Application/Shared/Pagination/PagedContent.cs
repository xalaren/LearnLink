using PandaRedu.Application.Shared.Pagination.Abstractions;

namespace PandaRedu.Application.Shared.Pagination;
/// <summary>
/// Represents a page of content with paging metadata.
/// </summary>
/// <typeparam name="TContent">The type of items contained in the page.</typeparam>
public record PagedContent<TContent>
(
    int Page,
    int PerPage,
    int Count,
    IReadOnlyCollection<TContent> Items
) : IPagedContent<TContent>
{   
    /// <inheritdoc />
    public int Pages => (int)Math.Ceiling(Count / (double)PerPage);

    /// <inheritdoc />
    public bool HasNextPage => Page < Pages;

    /// <inheritdoc />
    public bool HasPreviousPage => Page > 1;
}
