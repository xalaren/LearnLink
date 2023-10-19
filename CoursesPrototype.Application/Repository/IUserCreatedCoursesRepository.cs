using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface IUserCreatedCoursesRepository : IAsyncWriteRepository<UserCreatedCourse>
    {
        Task<Course[]> GetUserCreatedCoursesAsync(int userId);
    }
}
