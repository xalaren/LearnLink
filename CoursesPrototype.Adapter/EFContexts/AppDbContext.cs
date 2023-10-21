using CoursesPrototype.Adapter.EFConfigurations;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserCreatedCourse> UserCreatedCourses { get; set; }

        public DbSet<Module> Modules { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }

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
        }
    }
}
