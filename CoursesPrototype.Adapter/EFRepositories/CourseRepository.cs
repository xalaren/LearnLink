using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(Course entity)
        {
            await context.Courses.AddAsync(entity);
        }

        public async Task<Course?> GetAsync(int entityId)
        {
            return await context.Courses.FindAsync(entityId);
        }

        public async Task<Course[]> GetPublicAsync()
        {
            return await context.Courses.Where(course => course.IsPublic).ToArrayAsync();
        }

        public async Task RemoveAsync(int entityId)
        {
            var entity = await GetAsync(entityId);

            if(entity == null)
            {
                throw new NotFoundException("Курс не найден");
            }

            context.Courses.Remove(entity);
        }

        public void Update(Course entity)
        {
            context.Courses.Update(entity);
        }
    }
}
