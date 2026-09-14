namespace PandaRedu.Shared.Pagination;

/// <summary>
/// Base record for paged requests providing safe defaults and validation
/// for page and page size values.
/// </summary>
public abstract record PagedRequest() : IPagedRequest
{
    private readonly int page;
    private readonly int perPage = DefaultPageSize;

    public const int MinPage = 1;

    public const int MinPageSize = 1;
    public const int MaxPageSize = 100;
    private const int DefaultPageSize = 20;

    public int Page
    {
        get => page;
        init => page = Math.Max(value, MinPage);
    }

    public int PerPage
    {
        get => perPage;
        init => perPage = Math.Clamp(value, MinPageSize, MaxPageSize);
    }
    public int Skip => (Page - 1) * PerPage;
    public int Take => PerPage;
}