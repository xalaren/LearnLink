using LearnLink.Adapter.EFContexts;
using LearnLink.Application.Transaction;
using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Adapter.EFTransaction
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
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
        public DbSet<Section> Sections => context.Sections;
        public DbSet<LessonSection> LessonSections => context.LessonSections;

        public DbSet<LocalRole> LocalRoles => context.LocalRoles;
        public DbSet<CourseLocalRole> CourseLocalRoles => context.CourseLocalRoles;
        public DbSet<UserCourseLocalRole> UserCourseLocalRoles => context.UserCourseLocalRoles;

        public DbSet<CourseCompletion> CourseCompletions => context.CourseCompletions;
        public DbSet<ModuleCompletion> ModuleCompletions => context.ModuleCompletions;
        public DbSet<LessonCompletion> LessonCompletions => context.LessonCompletions;

        public DbSet<Objective> Objectives => context.Objectives;
        public DbSet<LessonObjective> LessonObjectives => context.LessonObjectives;
        public DbSet<Answer> Answers => context.Answers;
        public DbSet<Review> Reviews => context.Reviews;
        public DbSet<AnswerReview> AnswerReviews => context.AnswerReviews;

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
