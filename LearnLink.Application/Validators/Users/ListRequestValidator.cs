using FluentValidation;
using LearnLink.Shared.Model.Users;
using LearnLink.Shared.Pagination;

namespace LearnLink.Application.Validators.Users;

internal class ListRequestValidator : AbstractValidator<ListRequest>
{
    public ListRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(PagedRequest.MinPage)
            .WithMessage($"Page number must start from {PagedRequest.MinPage}");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(PagedRequest.MinPageSize, PagedRequest.MaxPageSize)
            .WithMessage($"Page size must be between {PagedRequest.MinPageSize} - {PagedRequest.MaxPageSize}");
    }
}
