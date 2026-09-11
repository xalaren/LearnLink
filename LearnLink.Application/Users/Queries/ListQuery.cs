using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Sorting;
using LearnLink.Shared.Pagination;

namespace LearnLink.Application.Users.Queries;

public record ListQuery(bool Descending, string? SortBy) : PagedRequest, ISortedRequest, IQuery;

public sealed class ListQueryValidator : AbstractValidator<ListQuery>
{
    public ListQueryValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(PagedRequest.MinPage)
            .WithMessage($"Page number must start from {PagedRequest.MinPage}");

        RuleFor(request => request.PerPage)
            .InclusiveBetween(PagedRequest.MinPageSize, PagedRequest.MaxPageSize)
            .WithMessage($"Page size must be between {PagedRequest.MinPageSize} - {PagedRequest.MaxPageSize}");
    }
}
