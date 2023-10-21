using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface ICourseModuleRepository : IAsyncWriteRepository<CourseModule>, IAsyncDisposable
    {
        Task<CourseModule[]> GetCourseModulesAsync();
        Task<CourseModule?> GetCourseModuleAsync(int courseId, int moduleId);
        Task<CourseModule[]> GetCourseModulesByCourseAsync(int courseId);
    }
}
