using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
namespace LearnLink.Application.Security.Commands;

/// <summary>
/// Command used to request a new access token using an existing refresh token.
/// </summary>
/// <param name="RefreshToken">The refresh token value.</param>
public sealed record RefreshCommand(string RefreshToken) : ICommand;

/// <summary>
/// Validator for <see cref="RefreshCommand"/> ensuring the refresh token is provided.
/// </summary>
public sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithName("refreshToken")
            .WithMessage("Refresh token is required");
    }
}

