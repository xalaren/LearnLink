using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class CoursesRepository : ICourseRepository
    {
        private readonly AppDbContext context;

        public CoursesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Course entity)
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

        public async Task<Course[]> GetByUserCreatedCoursesAsync(IEnumerable<UserCreatedCourse> userCreatedCourses)
        {
            List<Course> courses = new List<Course>();
            foreach (UserCreatedCourse userCreatedCourse in userCreatedCourses)
            {
                var course = await context.Courses.FirstOrDefaultAsync(course => course.Id == userCreatedCourse.CourseId);

                if (course != null) courses.Add(course);
            }
            return courses.ToArray();
        }

        public async Task<Course[]> GetSubscribedCourses(IEnumerable<Subscription> subscriptions)
        {
            List<Course> courses = new List<Course>();
            foreach (Subscription subscription in subscriptions)
            {
                var course = await context.Courses.FirstOrDefaultAsync(course => course.Id == subscription.CourseId);

                if (course != null) courses.Add(course);
            }
            return courses.ToArray();
        }

        public async Task RemoveAsync(int entityId)
        {
            var entity = await GetAsync(entityId);

            if (entity == null)
            {
                throw new NotFoundException("Курс не найден");
            }

            context.Courses.Remove(entity);
        }

        public void Update(Course entity)
        {
            context.Courses.Update(entity);
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public async Task<Course[]> GetCourses()
        {
            return await context.Courses.ToArrayAsync();
        }
    }
}
