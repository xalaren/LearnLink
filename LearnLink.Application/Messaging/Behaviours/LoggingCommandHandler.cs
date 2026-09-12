using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Messaging.Behaviours;

/// <summary>
/// Logging decorator for command handlers. Logs execution start, end and
/// errors for commands handled by the inner handler.
/// </summary>
/// <typeparam name="TCommand">The command type being handled.</typeparam>
public class LoggingCommandHandler<TCommand>
    (ILogger<TCommand> logger, ICommandHandler<TCommand> inner) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Handles the command by logging progress, delegating to the inner
    /// handler and logging the result or any unhandled exception.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response"/> representing the outcome.</returns>
    public async Task<Response> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType().Name;
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Command {Command} executing...", commandType);

            var response = await inner.Handle(command, cancellationToken);

            if(!response.IsSuccess)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Command {Command} execution ended with errors", commandType);
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError("Command {Command} execution ended with errors. Details:\n{Response}", commandType, response);
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Command {Command} execution ended with success", commandType);
            }

            return response;
        }
        catch(Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Command {Command} execution ended with errors", commandType);
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError(ex, "Unhandled exception handling {Command}", commandType);

            return new ResponseBuilder()
                .Fail()
                .WithMessage("Unknown error occured")
                .Build();
        }
    }
}

/// <summary>
/// Logging decorator for command handlers that return a typed result.
/// Logs execution start, end and errors for commands handled by the inner handler.
/// </summary>
/// <typeparam name="TCommand">The command type being handled.</typeparam>
/// <typeparam name="TResult">The result type returned by the handler.</typeparam>
public class LoggingCommandHandler<TCommand, TResult>
    (ILogger<TCommand> logger, ICommandHandler<TCommand, TResult> inner) : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    /// <summary>
    /// Handles the command by logging progress, delegating to the inner
    /// handler and logging the result or any unhandled exception.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Response{TResult}"/> representing the outcome.</returns>
    public async Task<Response<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType().Name;
        try
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Command {Command} executing...", commandType);

            var response = await inner.Handle(command, cancellationToken);

            if (!response.IsSuccess)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Command {Command} execution ended with errors", commandType);
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError("Command {Command} execution ended with errors. Details:\n{Response}", commandType, response);
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Command {Command} execution ended with success", commandType);
            }

            return response;
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Command {Command} execution ended with errors", commandType);
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError(ex, "Unhandled exception handling {Command}", commandType);

            return new ResponseBuilder<TResult>()
                .Fail()
                .WithMessage("Unknown error occured")
                .Build();
        }
    }
}
