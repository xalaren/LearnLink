namespace LearnLink.Application.Shared.Pagination.Abstractions;

public interface IPagedContent<TContent>
{
    int Page { get; }
    int PerPage { get; }
    int Count { get; }
    int Pages { get; }
    bool HasNextPage { get; }
    bool HasPreviousPage { get; }
    IReadOnlyCollection<TContent> Items { get; }
}