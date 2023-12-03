using LearnLink.Adapter.EFContexts;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.EFTransaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public DbSet<User> Users => context.Users;
        public DbSet<Credentials> Credentials => context.Credentials;
        public DbSet<Role> Roles => context.Roles;

        public DbSet<Course> Courses => context.Courses;
        public DbSet<Subscription> Subscriptions => context.Subscriptions;
        public DbSet<UserCreatedCourse> UserCreatedCourses => context.UserCreatedCourses;
        public DbSet<Module> Modules => context.Modules;
        public DbSet<CourseModule> CourseModules => context.CourseModules;
        public DbSet<Lesson> Lessons => context.Lessons;
        public DbSet<ModuleLesson> ModuleLessons => context.ModuleLessons;

        public ValueTask DisposeAsync()
        {
            return context.DisposeAsync();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
