namespace LearnLink.Shared.Pagination;

public readonly record struct PagedResponse<T>(
    int Page,
    int PerPage,
    int Count,
    IReadOnlyCollection<T> Items
): IPagedResponse<T>
{
    public int Pages => (int)Math.Ceiling(Count / (double)PerPage);
    public bool HasNextPage => Page < Pages;
    public bool HasPreviousPage => Page > 1;
}
