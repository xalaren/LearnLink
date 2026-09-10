using LearnLink.Application.Shared.Pagination.Abstractions;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Enums;

namespace LearnLink.Application.Shared.Pagination;
public record PagedContent<TContent>
(
    int Page,
    int PerPage,
    int Count,
    IReadOnlyCollection<TContent> Items
) : IPagedContent<TContent>
{   
    public int Pages => (int)Math.Ceiling(Count / (double)PerPage);
    public bool HasNextPage => Page < Pages;
    public bool HasPreviousPage => Page > 1;

    public IReadOnlyCollection<TContent>? Content { get; }
    public ResponseTypes ResponseType { get; }
    public bool IsSuccess { get; }
    public string? Message { get; }
    public Error[]? Details { get; }
}
