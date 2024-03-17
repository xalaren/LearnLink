using LearnLink.Adapter.EFConfigurations;
using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.EFContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LocalRole> LocalRoles { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserCreatedCourse> UserCreatedCourses { get; set; }
        public DbSet<UserCourseLocalRole> UserCourseLocalRoles { get; set; }

        public DbSet<Module> Modules { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<ModuleLesson> ModuleLessons { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserCreatedCoursesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModulesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseModulesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LessonsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleLessonsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LocalRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserCourseLocalRoleEntityTypeConfiguration());
        }
    }
}
