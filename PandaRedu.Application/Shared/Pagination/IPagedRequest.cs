namespace PandaRedu.Shared.Pagination;

/// <summary>
/// Represents a paged request that contains paging parameters used to
/// request a specific page of data.
/// </summary>
public interface IPagedRequest
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Items per page.
    /// </summary>
    int PerPage { get; }
}
