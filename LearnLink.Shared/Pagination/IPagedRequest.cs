namespace LearnLink.Shared.Pagination;

public interface IPagedRequest
{
    int Page { get; }
    int PerPage { get; }
}
