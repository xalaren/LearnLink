using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFRepositories
{
    public class UserCreatedCoursesRepository : IUserCreatedCoursesRepository
    {
        private readonly AppDbContext context;

        public UserCreatedCoursesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(UserCreatedCourse entity)
        {
            await context.UserCreatedCourses.AddAsync(entity);
        }

        public async Task<Course[]> GetUserCreatedCoursesAsync(int userId)
        {
            var userCourses = context.UserCreatedCourses.Where(userCourse => userCourse.UserId == userId);

            return await context.Courses.Where(course => userCourses.Any(e => e.CourseId == course.Id)).ToArrayAsync();
        }
    }
}
