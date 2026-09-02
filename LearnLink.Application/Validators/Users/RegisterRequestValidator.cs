using FluentValidation;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Users;

namespace LearnLink.Application.Validators.Users
{
    public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
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
        }
    }
}
