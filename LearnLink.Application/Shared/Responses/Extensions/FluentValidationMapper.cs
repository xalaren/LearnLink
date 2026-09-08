using FluentValidation.Results;

namespace LearnLink.Application.Shared.Responses.Extensions;

public static class FluentValidationMapper
{
    public static Error[] AsErrors(this ValidationResult validationResult)
    {
        return validationResult
            .Errors
            .Select(error => new Error(error.PropertyName, error.ErrorMessage))
            .ToArray();
    }
}
