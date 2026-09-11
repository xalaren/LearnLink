using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Users.Commands;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Users.CommandHandlers;

public class RegisterCommandHandler(IApplicationDataContext repository, IEncryptionProvider encryptionProvider) : ICommandHandler<RegisterCommand>
{
    public async Task<Response> Handle(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder();

        var exists = await repository
                    .Users
                    .AsNoTracking()
                    .AnyAsync(user => user.Nickname == command.Nickname, cancellationToken);

        if (exists) return responseBuilder
                .Conflict()
                .WithMessage($"User with nickname \"{command.Nickname}\" is already exists")
                .Build();

        var user = User.Create(command.Nickname, command.Name, command.Lastname);

        var encryptedPassword = encryptionProvider.Encrypt(command.Password);
        var credentials = Credentials.Create(encryptedPassword, user.Id, false, command.PasswordExpiration);

        repository.Users.Add(user);
        repository.Credentials.Add(credentials);

        await repository.CommitAsync(cancellationToken);

        return responseBuilder
            .Succeed()
            .WithMessage("User registered successfully")
            .Build();
    }
}
