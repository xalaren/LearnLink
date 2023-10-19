using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;

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

        public Task<Course?> GetAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task Remove(int entityId)
        {
            throw new NotImplementedException();
        }

        public void Update(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
