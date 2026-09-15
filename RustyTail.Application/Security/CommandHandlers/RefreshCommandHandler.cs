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
/// Handles <see cref="RefreshCommand"/> to exchange a valid refresh token
/// for a new access and refresh token pair.
/// </summary>
public sealed class RefreshCommandHandler(
    IApplicationDataContext repository,
    ITokenProvider tokenProvider
) : ICommandHandler<RefreshCommand, TokenPair>
{
    /// <summary>
    /// Processes the refresh command and returns a new <see cref="TokenPair"/> on success.
    /// </summary>
    /// <param name="command">The refresh command containing the refresh token.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Response{TokenPair}"/> describing the outcome.</returns>
    public async Task<Response<TokenPair>> Handle(RefreshCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TokenPair>();

        RefreshToken? refreshToken = await repository
            .RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == command.RefreshToken, cancellationToken);

        if(refreshToken is null || refreshToken.IsExpired)
        {
            return responseBuilder
                .Forbid()
                .WithMessage("Refresh token has expired")
                .Build();
        }

        var tokenPair = new TokenPair
        (
            AccessToken: tokenProvider.GenerateAccessToken(refreshToken.User),
            RefreshToken: tokenProvider.GenerateRefreshToken()
        );

        refreshToken.Refresh(tokenPair.RefreshToken);

        await repository.CommitAsync(cancellationToken);

        return responseBuilder
            .Succeed()
            .WithContent(tokenPair)
            .Build();
    }
}
