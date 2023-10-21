using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface IUserRepository : IAsyncRepository<User>, IAsyncDisposable
    {
        Task<User?> GetByNicknameAsync(string nickname);
        Task<User[]> GetAll();
    }
}
