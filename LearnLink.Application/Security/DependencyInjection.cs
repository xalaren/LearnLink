using LearnLink.Application.Messaging;
using LearnLink.Application.Security.CommandHandlers;
using LearnLink.Application.Security.Commands;
using LearnLink.Application.Security.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LearnLink.Application.Security;

internal static class DependencyInjection
{
    internal static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        return services
            .AddCommandHandler<LoginCommandHandler, LoginCommand, TokenPair, LoginCommandValidator>();
    }
}
