using FluentValidation;
using PandaRedu.Application.Abstractions.Messaging;
using PandaRedu.Application.Shared.Sorting;
using PandaRedu.Shared.Pagination;

namespace PandaRedu.Application.Users.Queries;

/// <summary>
/// Query used to request a paged list of users with optional sorting.
/// </summary>
/// <param name="Descending">Whether results should be sorted in descending order.</param>
/// <param name="SortBy">Optional field name to sort by.</param>
public record ListQuery(bool Descending, string? SortBy) : PagedRequest, ISortedRequest, IQuery;

/// <summary>
/// Validator for <see cref="ListQuery"/> that ensures paging parameters are within allowed ranges.
/// </summary>
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
