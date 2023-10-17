using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User?> GetByNickname(string nickname);
        Task<User[]> GetAll();
    }
}
