using FluentValidation;
using LearnLink.Application.Security.Models;

namespace LearnLink.Application.Security.Validators;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    internal LoginRequestValidator()
    {
        RuleFor(loginRequest => loginRequest.Nickname)
            .NotEmpty()
            .WithName("nickname")
            .WithMessage("Nickname is required");

        RuleFor(loginRequest => loginRequest.Nickname)
            .NotEmpty()
            .WithName("passsword")
            .WithMessage("Password is required");
    }
}
