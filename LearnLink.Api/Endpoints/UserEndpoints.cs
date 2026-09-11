using LearnLink.Api.Mappers.ResponseToResult;
using LearnLink.Application.Messaging.Abstractions;
using LearnLink.Application.Users.Commands;

namespace LearnLink.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", Register);
    }

    public static async Task<IResult> Register
    (
        RegisterCommand command,
        ICommandHandler<RegisterCommand> commandHandler,
        CancellationToken cancellationToken = default
    )
    {
        var response = await commandHandler.Handle(command, cancellationToken);
        return response.ToResult();
    }
}
