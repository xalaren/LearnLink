using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface ICourseRepository : IAsyncRepository<Course>
    {
        Task<Course[]> GetPublicAsync();
    }
}
