using LearnLink.Shared.Pagination;
using LearnLink.Shared.Sorting;

namespace LearnLink.Application.Users.Models;

public record ListRequest(bool Descending, string? SortBy) : PagedRequest, ISortedRequest;