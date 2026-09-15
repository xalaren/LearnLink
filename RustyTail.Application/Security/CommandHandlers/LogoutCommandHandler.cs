using Microsoft.EntityFrameworkCore;
using RustyTail.Application.Abstractions.Data;
using RustyTail.Application.Abstractions.Messaging;
using RustyTail.Application.Security.Commands;
using RustyTail.Application.Security.Contexts;
using RustyTail.Application.Shared.Responses;

namespace RustyTail.Application.Security.CommandHandlers;

public class LogoutCommandHandler
(
    IApplicationDataContext repository,
    ICurrentUserContext userContext
): ICommandHandler<LogoutCommand>
{
    public async Task<Response> Handle(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder();

        var currentUser = userContext.GetUserId();

        if(currentUser == null)
        {
            return responseBuilder
                .Forbid()
                .WithMessage("This action is unavailable for you")
                .Build();
        }

        await repository
            .RefreshTokens
            .Where(rt => rt.Token == command.RefreshToken && rt.UserId == currentUser)
            .ExecuteDeleteAsync(cancellationToken);

        return responseBuilder
            .Succeed()
            .WithMessage("User logged out successfully")
            .Build();
    }
}
