using PandaRedu.Application.Shared.Responses;

namespace PandaRedu.Application.Abstractions.Messaging;

/// <summary>
/// Handles a command of type <typeparamref name="TCommand"/> and returns a
/// <see cref="Response"/> indicating success or failure.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Processes the specified command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the handling operation. The task result contains a <see cref="Response"/>.</returns>
    Task<Response> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handles a command of type <typeparamref name="TCommand"/> and returns a
/// <see cref="Response{TResult}"/> containing a result value.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
/// <typeparam name="TResult">The type of the result returned in the response.</typeparam>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand
{
    /// <summary>
    /// Processes the specified command and returns a typed result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the handling operation. The task result contains a <see cref="Response{TResult}"/>.</returns>
    Task<Response<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
