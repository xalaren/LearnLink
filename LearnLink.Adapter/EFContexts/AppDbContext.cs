using LearnLink.Adapter.EFConfigurations;
using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.EFContexts
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Credentials> Credentials { get; init; }
        public DbSet<Role> Roles { get; init; }
        public DbSet<LocalRole> LocalRoles { get; init; }

        public DbSet<Course> Courses { get; init; }
        public DbSet<Subscription> Subscriptions { get; init; }
        public DbSet<UserCreatedCourse> UserCreatedCourses { get; init; }
        public DbSet<CourseLocalRole> CourseLocalRoles { get; init; }
        public DbSet<UserCourseLocalRole> UserCourseLocalRoles { get; init; }

        public DbSet<Module> Modules { get; init; }
        public DbSet<CourseModule> CourseModules { get; init; }

        public DbSet<Lesson> Lessons { get; init; }
        public DbSet<Section> Sections { get; init; }

        public DbSet<LessonSection> LessonSections { get; init; }
        public DbSet<ModuleLesson> ModuleLessons { get; init; }

        public DbSet<CourseCompletion> CourseCompletions { get; init; }
        public DbSet<ModuleCompletion> ModuleCompletions { get; init; }
        public DbSet<LessonCompletion> LessonCompletions { get; init; }

        public DbSet<Objective> Objectives { get; init; }
        public DbSet<LessonObjective> LessonObjectives { get; init; }

        public DbSet<Answer> Answers { get; init; }
        public DbSet<Review> Reviews { get; init; }
        public DbSet<AnswerReview> AnswerReviews { get; init; }

        public DbSet<TextContent> TextContents { get; init; }
        public DbSet<CodeContent> CodeContents { get; init; }
        public DbSet<FileContent> FileContents { get; init; }

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
            modelBuilder.ApplyConfiguration(new CourseLocalRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserCourseLocalRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseCompletionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleComletionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SectionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LessonCompletionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LessonSectionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectiveEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LessonObjectiveEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerReviewEntityTypeConfiguration());
        }
    }
}
