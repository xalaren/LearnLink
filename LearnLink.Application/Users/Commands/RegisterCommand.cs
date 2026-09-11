using LearnLink.Application.Messaging.Abstractions;

namespace LearnLink.Application.Users.Commands;

public record RegisterCommand(string Nickname, string Name, string Lastname, string Password, DateTime? PasswordExpiration) : ICommand;
