using LearnLink.Shared.Pagination;
using LearnLink.Shared.Sorting;

namespace LearnLink.Shared.Model.Users;

public record ListRequest(bool Descending, string? SortBy) : PagedRequest, ISortedRequest;