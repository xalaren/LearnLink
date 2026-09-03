namespace LearnLink.Shared.Pagination;

public record PagedRequest() : IPagedRequest
{
    private int _page;
    private int _pageSize = DefaultPageSize;

    public const int MinPage = 1;

    public const int MinPageSize = 1;
    public const int MaxPageSize = 100;
    public const int DefaultPageSize = 20;

    public int Page
    {
        get => _page;
        init => _page = Math.Max(value, MinPage);
    }

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Clamp(value, MinPageSize, MaxPageSize);
    }
    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}