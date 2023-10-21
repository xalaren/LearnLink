using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class CourseModuleRepository : ICourseModuleRepository
    {
        private readonly AppDbContext context;

        public CourseModuleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(CourseModule entity)
        {
            await context.CourseModules.AddAsync(entity);
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public async Task<CourseModule?> GetCourseModuleAsync(int courseId, int moduleId)
        {
            return await context.CourseModules.FirstOrDefaultAsync(courseModule => courseModule.CourseId == courseId && courseModule.ModuleId == moduleId);
        }

        public async Task<CourseModule[]> GetCourseModulesAsync()
        {
            return await context.CourseModules.ToArrayAsync();
        }

        public async Task<CourseModule[]> GetCourseModulesByCourseAsync(int courseId)
        {
            return await context.CourseModules.Where(courseModule => courseModule.CourseId == courseId).ToArrayAsync();
        }
    }
}
