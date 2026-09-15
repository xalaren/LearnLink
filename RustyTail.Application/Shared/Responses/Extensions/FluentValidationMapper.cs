using FluentValidation.Results;

namespace RustyTail.Application.Shared.Responses.Extensions;

public static class FluentValidationMapper
{
    /// <summary>
    /// Maps a FluentValidation <see cref="ValidationResult"/> to an array of
    /// <see cref="Error"/> instances usable by the application's response types.
    /// </summary>
    /// <param name="validationResult">The validation result to map.</param>
    /// <returns>An array of <see cref="Error"/> representing validation failures.</returns>
    public static Error[] AsErrors(this ValidationResult validationResult)
    {
        return validationResult
            .Errors
            .Select(error => new Error(error.PropertyName, error.ErrorMessage))
            .ToArray();
    }
}
