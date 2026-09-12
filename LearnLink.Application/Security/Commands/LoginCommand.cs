using FluentValidation;
using LearnLink.Application.Abstractions.Messaging;

namespace LearnLink.Application.Security.Commands;

public sealed record LoginCommand(string Nickname, string Password) : ICommand;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(loginRequest => loginRequest.Nickname)
            .NotEmpty()
            .WithName("nickname")
            .WithMessage("Nickname is required");

        RuleFor(loginRequest => loginRequest.Password)
            .NotEmpty()
            .WithName("passsword")
            .WithMessage("Password is required");
    }
}
