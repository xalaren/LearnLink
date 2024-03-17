using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    internal class ModuleLessonsEntityTypeConfiguration : IEntityTypeConfiguration<ModuleLesson>
    {
        public void Configure(EntityTypeBuilder<ModuleLesson> builder)
        {
            builder.HasKey(moduleLesson => new { moduleLesson.ModuleId, moduleLesson.LessonId });

            //builder
            //    .HasOne(ml => ml.Module)
            //    .WithMany(m => m.ModuleLessons)
            //    .HasForeignKey(ml => ml.ModuleId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder
            //    .HasOne(ml => ml.Lesson)
            //    .WithOne(l => l.ModuleLesson)
            //    .HasForeignKey<ModuleLesson>(ml => ml.LessonId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
