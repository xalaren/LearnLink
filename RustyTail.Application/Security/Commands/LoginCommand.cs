using FluentValidation;
using RustyTail.Application.Abstractions.Messaging;

namespace RustyTail.Application.Security.Commands;

/// <summary>
/// Command used to request authentication (login) by nickname and password.
/// </summary>
/// <param name="Nickname">User nickname.</param>
/// <param name="Password">User password.</param>
public sealed record LoginCommand(string Nickname, string Password) : ICommand;

/// <summary>
/// Validator for <see cref="LoginCommand"/> ensuring required fields are provided.
/// </summary>
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
