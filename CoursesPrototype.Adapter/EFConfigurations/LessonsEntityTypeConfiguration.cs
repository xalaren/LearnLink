using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesPrototype.Adapter.EFConfigurations
{
    public class LessonsEntityTypeConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.Property(lesson => lesson.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(lesson => lesson.Description)
                .HasMaxLength(500);


        }
    }
}
