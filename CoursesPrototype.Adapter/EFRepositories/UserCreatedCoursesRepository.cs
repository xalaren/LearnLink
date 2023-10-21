using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Repository;
using CoursesPrototype.Core.Entities;
using CoursesPrototype.Core.Exceptions;
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

        public async Task CreateAsync(UserCreatedCourse entity)
        {
            await context.UserCreatedCourses.AddAsync(entity);
        }

        public async Task<UserCreatedCourse[]> GetUserCreatedCoursesAsync(int userId)
        {
            return await context.UserCreatedCourses.Where(userCourse => userCourse.UserId == userId).ToArrayAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public async Task<UserCreatedCourse?> GetUserCreatedCourse(int userId, int courseId)
        {
            return await context.UserCreatedCourses.FirstOrDefaultAsync(u => u.UserId == userId && u.CourseId == courseId);
        }
    }
}
