using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Transaction
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public DbSet<User> Users { get; }
        public DbSet<Credentials> Credentials { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<LocalRole> LocalRoles { get; }

        public DbSet<Course> Courses { get; }
        public DbSet<Subscription> Subscriptions { get; }
        public DbSet<UserCreatedCourse> UserCreatedCourses { get; }
        public DbSet<UserCourseLocalRole> UserCourseLocalRoles { get; }

        public DbSet<Module> Modules { get; }
        public DbSet<CourseModule> CourseModules { get; }

        public DbSet<Lesson> Lessons { get; }
        public DbSet<ModuleLesson> ModuleLessons { get; }

        Task CommitAsync();
    }
}
