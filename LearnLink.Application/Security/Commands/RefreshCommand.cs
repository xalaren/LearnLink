using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;
namespace LearnLink.Application.Security.Commands;

public sealed record RefreshCommand(string RefreshToken) : ICommand;

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

