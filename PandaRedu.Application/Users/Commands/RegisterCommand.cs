using FluentValidation;
using PandaRedu.Application.Abstractions.Messaging;
using PandaRedu.Domain.Entities.Users.Models;

namespace PandaRedu.Application.Users.Commands;

/// <summary>
/// Command used to register a new user in the system.
/// </summary>
/// <param name="Nickname">User's nickname (unique).</param>
/// <param name="Name">First name.</param>
/// <param name="Lastname">Last name.</param>
/// <param name="Password">Plain-text password to be encrypted and stored.</param>
public record RegisterCommand(string Nickname, string Name, string Lastname, string Password) : ICommand;

/// <summary>
/// Validator for <see cref="RegisterCommand"/> ensuring required fields and length constraints.
/// </summary>
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
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
