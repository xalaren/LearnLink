using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Security.Commands;
using LearnLink.Application.Security.Models;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Shared.Responses;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Security.CommandHandlers;

public sealed class RefreshCommandHandler(
    IApplicationDataContext repository,
    ITokenProvider tokenProvider
) : ICommandHandler<RefreshCommand, TokenPair>
{
    public async Task<Response<TokenPair>> Handle(RefreshCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TokenPair>();

        RefreshToken? refreshToken = await repository
            .RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == command.RefreshToken);

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

        await repository.CommitAsync();

        return responseBuilder
            .Succeed()
            .WithContent(tokenPair)
            .Build();
    }
}
