namespace LearnLink.Shared.Pagination;

public readonly record struct PagedResponse<T>
(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<T> Items
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
