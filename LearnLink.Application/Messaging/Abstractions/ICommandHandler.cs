using LearnLink.Application.Shared.Responses;

namespace LearnLink.Application.Messaging.Abstractions;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Response> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand
{
    Task<Response<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
