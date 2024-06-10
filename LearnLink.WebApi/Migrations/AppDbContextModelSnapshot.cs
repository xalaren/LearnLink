﻿// <auto-generated />
using System;
using LearnLink.Adapter.EFContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LearnLink.WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LearnLink.Core.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FileContentId")
                        .HasColumnType("integer");

                    b.Property<int>("ObjectiveId")
                        .HasColumnType("integer");

                    b.Property<int?>("TextContentId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FileContentId");

                    b.HasIndex("ObjectiveId");

                    b.HasIndex("TextContentId");

                    b.HasIndex("UserId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.AnswerReview", b =>
                {
                    b.Property<int>("AnswerId")
                        .HasColumnType("integer");

                    b.Property<int>("ReviewId")
                        .HasColumnType("integer");

                    b.HasKey("AnswerId", "ReviewId");

                    b.HasIndex("ReviewId");

                    b.ToTable("AnswerReviews");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ContentEntities.CodeContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeLanguage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CodeText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CodeContent");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ContentEntities.FileContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FileContent");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ContentEntities.TextContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TextContent");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUnavailable")
                        .HasColumnType("boolean");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseCompletion", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<int>("CompletionProgress")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseCompletions");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseLocalRole", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("LocalRoleId")
                        .HasColumnType("integer");

                    b.HasKey("CourseId", "LocalRoleId");

                    b.HasIndex("LocalRoleId");

                    b.ToTable("CourseLocalRoles");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseModule", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("ModuleId")
                        .HasColumnType("integer");

                    b.HasKey("CourseId", "ModuleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("CourseModules");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonCompletion", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("ModuleId")
                        .HasColumnType("integer");

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<int>("CompletionProgress")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "ModuleId", "LessonId");

                    b.HasIndex("LessonId");

                    b.HasIndex("ModuleId");

                    b.ToTable("LessonCompletions");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonObjective", b =>
                {
                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<int>("ObjectiveId")
                        .HasColumnType("integer");

                    b.HasKey("LessonId", "ObjectiveId");

                    b.HasIndex("ObjectiveId");

                    b.ToTable("LessonObjectives");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonSection", b =>
                {
                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<int>("SectionId")
                        .HasColumnType("integer");

                    b.HasKey("LessonId", "SectionId");

                    b.HasIndex("SectionId");

                    b.ToTable("LessonSections");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LocalRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("EditAcess")
                        .HasColumnType("boolean");

                    b.Property<bool>("EditRolesAccess")
                        .HasColumnType("boolean");

                    b.Property<bool>("InviteAccess")
                        .HasColumnType("boolean");

                    b.Property<bool>("KickAccess")
                        .HasColumnType("boolean");

                    b.Property<bool>("ManageInternalAccess")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("RemoveAccess")
                        .HasColumnType("boolean");

                    b.Property<string>("Sign")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("SystemRole")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewAccess")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("Sign")
                        .IsUnique();

                    b.ToTable("LocalRoles");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ModuleCompletion", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("ModuleId")
                        .HasColumnType("integer");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<int>("CompletionProgress")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CourseId", "ModuleId");

                    b.HasIndex("CourseId");

                    b.HasIndex("ModuleId");

                    b.ToTable("ModuleCompletions");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ModuleLesson", b =>
                {
                    b.Property<int>("ModuleId")
                        .HasColumnType("integer");

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.HasKey("ModuleId", "LessonId");

                    b.HasIndex("LessonId");

                    b.ToTable("ModuleLessons");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Objective", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FileContentId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("FileContentId");

                    b.ToTable("Objectives");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<int>("ExpertUserId")
                        .HasColumnType("integer");

                    b.Property<int>("Grade")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExpertUserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sign")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CodeContentId")
                        .HasColumnType("integer");

                    b.Property<int?>("FileContentId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int?>("TextContentId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CodeContentId");

                    b.HasIndex("FileContentId");

                    b.HasIndex("TextContentId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Subscription", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarFileName")
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Nickname")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.UserCourseLocalRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("LocalRoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CourseId", "LocalRoleId");

                    b.HasIndex("CourseId");

                    b.HasIndex("LocalRoleId");

                    b.ToTable("UserCourseLocalRoles");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.UserCreatedCourse", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCreatedCourses");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Answer", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.ContentEntities.FileContent", "FileContent")
                        .WithMany()
                        .HasForeignKey("FileContentId");

                    b.HasOne("LearnLink.Core.Entities.Objective", "Objective")
                        .WithMany()
                        .HasForeignKey("ObjectiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.ContentEntities.TextContent", "TextContent")
                        .WithMany()
                        .HasForeignKey("TextContentId");

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileContent");

                    b.Navigation("Objective");

                    b.Navigation("TextContent");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.AnswerReview", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Review", "Review")
                        .WithMany()
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseCompletion", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseLocalRole", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.LocalRole", "LocalRole")
                        .WithMany()
                        .HasForeignKey("LocalRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("LocalRole");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.CourseModule", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Credentials", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonCompletion", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Module");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonObjective", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Objective", "Objective")
                        .WithMany()
                        .HasForeignKey("ObjectiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Objective");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LessonSection", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ModuleCompletion", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Module");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.ModuleLesson", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Objective", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.ContentEntities.FileContent", "FileContent")
                        .WithMany()
                        .HasForeignKey("FileContentId");

                    b.Navigation("FileContent");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Review", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.User", "ExpertUser")
                        .WithMany()
                        .HasForeignKey("ExpertUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpertUser");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Section", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.ContentEntities.CodeContent", "CodeContent")
                        .WithMany()
                        .HasForeignKey("CodeContentId");

                    b.HasOne("LearnLink.Core.Entities.ContentEntities.FileContent", "FileContent")
                        .WithMany()
                        .HasForeignKey("FileContentId");

                    b.HasOne("LearnLink.Core.Entities.ContentEntities.TextContent", "TextContent")
                        .WithMany()
                        .HasForeignKey("TextContentId");

                    b.Navigation("CodeContent");

                    b.Navigation("FileContent");

                    b.Navigation("TextContent");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Subscription", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.User", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.UserCourseLocalRole", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.LocalRole", "LocalRole")
                        .WithMany()
                        .HasForeignKey("LocalRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("LocalRole");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.UserCreatedCourse", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnLink.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
