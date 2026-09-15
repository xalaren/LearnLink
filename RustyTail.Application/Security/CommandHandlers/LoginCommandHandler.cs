using RustyTail.Application.Abstractions.Data;
using RustyTail.Application.Abstractions.Messaging;
using RustyTail.Application.Security.Commands;
using RustyTail.Application.Security.Models;
using RustyTail.Application.Security.Providers;
using RustyTail.Application.Shared.Responses;
using RustyTail.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace RustyTail.Application.Security.CommandHandlers;

/// <summary>
/// Handles <see cref="LoginCommand"/>, validating credentials and
/// issuing a <see cref="TokenPair"/> when authentication succeeds.
/// </summary>
/// <remarks>
/// This handler verifies the provided password using <see cref="IEncryptionProvider"/>
/// and generates tokens via <see cref="ITokenProvider"/>. A new refresh token
/// entity is persisted to the data context upon successful login.
/// </remarks>
public sealed class LoginCommandHandler(
    IApplicationDataContext repository,
    IEncryptionProvider encryptionProvider,
    ITokenProvider tokenProvider
) : ICommandHandler<LoginCommand, TokenPair>
{
    /// <summary>
    /// Processes the login command and returns a token pair on success.
    /// </summary>
    /// <param name="command">The login command containing user credentials.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Response{TokenPair}"/> describing the outcome.</returns>
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
