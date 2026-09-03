using LearnLink.Application.Repositories;

namespace LearnLink.Application.Transactions
{
    public interface IUnitOfWork
    {
        IAppRepository Repository { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
