using PandaRedu.Application.Abstractions.Data;
using PandaRedu.Application.Abstractions.Messaging;
using PandaRedu.Application.Security.Providers;
using PandaRedu.Application.Shared.Responses;
using PandaRedu.Application.Users.Commands;
using PandaRedu.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace PandaRedu.Application.Users.CommandHandlers;

/// <summary>
/// Handles <see cref="RegisterCommand"/> by creating a new user and credentials
/// after validating uniqueness and encrypting the password.
/// </summary>
public class RegisterCommandHandler(IApplicationDataContext repository, IEncryptionProvider encryptionProvider) : ICommandHandler<RegisterCommand>
{
    /// <summary>
    /// Processes the register command and returns a response indicating the result.
    /// </summary>
    /// <param name="command">The register command with user details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Response"/> describing the outcome.</returns>
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
        var credentials = Credentials.Create(encryptedPassword, user.Id, false);

        repository.Users.Add(user);
        repository.Credentials.Add(credentials);

        await repository.CommitAsync(cancellationToken);

        return responseBuilder
            .Succeed()
            .WithMessage("User registered successfully")
            .Build();
    }
}
