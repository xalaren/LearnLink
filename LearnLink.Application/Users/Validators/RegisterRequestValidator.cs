using FluentValidation;
using LearnLink.Application.Users.Models;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Users.Validators;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    internal RegisterRequestValidator()
    {
        RuleFor(request => request.Nickname)
            .NotEmpty()
            .WithMessage("Nickname is required")
            .MaximumLength(User.NicknameMaxLength)
            .WithMessage($"Nickname cannot exceed {User.NicknameMaxLength} characters");

        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(User.NameMaxLength)
            .WithMessage($"Name cannot exceed {User.NameMaxLength} characters");

        RuleFor(request => request.Lastname)
            .NotEmpty()
            .WithMessage("Lastname is required")
            .MaximumLength(User.LastnameMaxLength)
            .WithMessage($"Lastname cannot exceed {User.LastnameMaxLength} characters");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(Credentials.PasswordMinLength)
            .WithMessage($"Password must have at least {Credentials.PasswordMinLength} characters");
    }
}
