using LearnLink.Application.Shared.Responses;

namespace LearnLink.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    Task<Response<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
