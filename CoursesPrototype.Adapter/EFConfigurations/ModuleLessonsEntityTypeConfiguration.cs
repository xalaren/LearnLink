using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesPrototype.Adapter.EFConfigurations
{
    internal class ModuleLessonsEntityTypeConfiguration : IEntityTypeConfiguration<ModuleLesson>
    {
        public void Configure(EntityTypeBuilder<ModuleLesson> builder)
        {
            builder.HasKey(moduleLesson => new { moduleLesson.ModuleId, moduleLesson.LessonId });
        }
    }
}
