using CoursesPrototype.Application.Repository.BasicRepositories;
using CoursesPrototype.Core.Entities;

namespace CoursesPrototype.Application.Repository
{
    public interface IModulesRepository : IAsyncWriteRepository<Module>, IAsyncReadRepository<Module>, IAsyncRemoveRepository, IUpdateRepository<Module>, IAsyncDisposable
    {
        Task<Module[]> GetAllModulesAsync();
        Task<Module[]> GetModulesFromCourseModules(IEnumerable<CourseModule> courseModules);
    }
}
