﻿// <auto-generated />
using System;
using LearnLink.Adapter.EFContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            modelBuilder.Entity("LearnLink.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsUnavailable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModuleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CourseId", "ModuleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("CourseModules");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("LearnLink.Core.Entities.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("LessonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ModuleId", "LessonId");

                    b.HasIndex("LessonId");

                    b.ToTable("ModuleLessons");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sign")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Role");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeLanguage")
                        .HasColumnType("text");

                    b.Property<string>("FileExtension")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<bool>("IsCodeBlock")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFile")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsText")
                        .HasColumnType("boolean");

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.Subscription", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarFileName")
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCreatedCourses");
                });

            modelBuilder.Entity("LearnLink.Core.Entities.LocalRole", b =>
                {
                    b.HasBaseType("LearnLink.Core.Entities.Role");

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

                    b.Property<bool>("RemoveAccess")
                        .HasColumnType("boolean");

                    b.Property<bool>("SystemRole")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewAccess")
                        .HasColumnType("boolean");

                    b.HasIndex("Sign")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("LocalRole");
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

            modelBuilder.Entity("LearnLink.Core.Entities.Section", b =>
                {
                    b.HasOne("LearnLink.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
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
