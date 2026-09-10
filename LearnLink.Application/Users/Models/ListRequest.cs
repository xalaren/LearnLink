using LearnLink.Application.Shared.Sorting;
using LearnLink.Shared.Pagination;

namespace LearnLink.Application.Users.Models;

public record ListRequest(bool Descending, string? SortBy) : PagedRequest, ISortedRequest;