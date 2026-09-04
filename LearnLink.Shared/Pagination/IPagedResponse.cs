namespace LearnLink.Shared.Pagination;

public interface IPagedResponse<T>
{
    int Page { get; }
    int PerPage { get; }
    int Count { get; }
    int Pages { get; }
    bool HasNextPage { get; }
    bool HasPreviousPage { get; }
    IReadOnlyCollection<T> Items { get; }
}