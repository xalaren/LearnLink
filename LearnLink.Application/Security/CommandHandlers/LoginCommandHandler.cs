using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Security.Commands;
using LearnLink.Application.Security.Models;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Shared.Responses;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Security.CommandHandlers;

public sealed class LoginCommandHandler(
    IApplicationDataContext repository,
    IEncryptionProvider encryptionProvider,
    ITokenProvider tokenProvider
) : ICommandHandler<LoginCommand, TokenPair>
{
    public async Task<Response<TokenPair>> Handle(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TokenPair>();

        var credentials = await repository
                .Credentials
                .Include(creds => creds.User)
                .ThenInclude(user => user.Role)
                .FirstOrDefaultAsync(creds => creds.User.Nickname == command.Nickname, cancellationToken);

        if (credentials == null)
        {
            return responseBuilder
                .NotFound()
                .WithMessage("User not exists")
                .Build();
        }

        var verified = encryptionProvider.Verify(command.Password, credentials.Password);

        if (!verified)
        {
            return responseBuilder
                .Forbid()
                .WithMessage("User nickname or password are incorrect")
                .Build();
        }

        var user = credentials.User;
        var tokenPair = new TokenPair
        (
            AccessToken: tokenProvider.GenerateAccessToken(user),
            RefreshToken: tokenProvider.GenerateRefreshToken()
        );

        repository
            .RefreshTokens
            .Add(RefreshToken.Create(user.Id, tokenPair.RefreshToken));

        await repository.CommitAsync(cancellationToken);

        return responseBuilder
            .Succeed()
            .WithMessage("User logged in successfully")
            .WithContent(tokenPair)
            .Build();
    }
}
