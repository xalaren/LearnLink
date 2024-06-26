﻿using LearnLink.Core.Entities;
using LearnLink.Core.Entities.ContentEntities;
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
        public DbSet<CourseLocalRole> CourseLocalRoles { get; }
        public DbSet<UserCourseLocalRole> UserCourseLocalRoles { get; }

        public DbSet<Module> Modules { get; }
        public DbSet<CourseModule> CourseModules { get; }

        public DbSet<Lesson> Lessons { get; }

        public DbSet<LessonSection> LessonSections { get; }
        public DbSet<ModuleLesson> ModuleLessons { get; }
        public DbSet<Section> Sections { get; }

        public DbSet<CourseCompletion> CourseCompletions { get; }
        public DbSet<ModuleCompletion> ModuleCompletions { get; }
        public DbSet<LessonCompletion> LessonCompletions { get; }

        public DbSet<Objective> Objectives { get; }
        public DbSet<LessonObjective> LessonObjectives { get; }
        public DbSet<Answer> Answers { get; }
        public DbSet<Review> Reviews { get; }
        public DbSet<AnswerReview> AnswerReviews { get; }

        public DbSet<TextContent> TextContents { get; }
        public DbSet<CodeContent> CodeContents { get; }
        public DbSet<FileContent> FileContents { get; }

        Task CommitAsync();
    }
}
