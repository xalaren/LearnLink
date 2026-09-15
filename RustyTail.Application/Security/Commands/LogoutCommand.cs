
using FluentValidation;
using RustyTail.Application.Abstractions.Messaging;

namespace RustyTail.Application.Security.Commands;

/// <summary>
/// Command used to logging out a user
/// </summary>
/// <param name="RefreshToken">Respective refresh token, which presents user's current session</param>
public record LogoutCommand(string RefreshToken) : ICommand;

/// <summary>
/// Validator for <see cref="LogoutCommand"/>
/// </summary>
public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithName("refreshToken")
            .WithMessage("Refresh token is required");
    }
}
