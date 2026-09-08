using FluentValidation;
using LearnLink.Application.Users.Models;
using LearnLink.Shared.Pagination;

namespace LearnLink.Application.Users.Validators;

internal class ListRequestValidator : AbstractValidator<ListRequest>
{
    public ListRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(PagedRequest.MinPage)
            .WithMessage($"Page number must start from {PagedRequest.MinPage}");

        RuleFor(request => request.PerPage)
            .InclusiveBetween(PagedRequest.MinPageSize, PagedRequest.MaxPageSize)
            .WithMessage($"Page size must be between {PagedRequest.MinPageSize} - {PagedRequest.MaxPageSize}");
    }
}
