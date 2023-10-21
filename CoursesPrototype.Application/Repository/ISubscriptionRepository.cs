using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Shared.DataTransferObjects;

namespace CoursesPrototype.Application.Repository
{
    public interface ISubscriptionRepository : IAsyncWriteRepository<Subscription>, IAsyncDisposable
    {
        Task<Subscription[]> GetUserSubscriptions(int userId);
        Task RemoveAsync(int userId, int courseId);
    }
}