namespace PandaRedu.Application.Shared.Pagination.Abstractions;

/// <summary>
/// Represents paged content returned by list queries. Provides paging
/// metadata and the items for the current page.
/// </summary>
/// <typeparam name="TContent">The type of items contained in the page.</typeparam>
public interface IPagedContent<TContent>
{
    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    int PerPage { get; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    int Pages { get; }

    /// <summary>
    /// Whether a next page exists.
    /// </summary>
    bool HasNextPage { get; }

    /// <summary>
    /// Whether a previous page exists.
    /// </summary>
    bool HasPreviousPage { get; }

    /// <summary>
    /// The items for the current page.
    /// </summary>
    IReadOnlyCollection<TContent> Items { get; }
}
