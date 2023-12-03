using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class CoursesEntityTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(course => course.Title)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(course => course.Description)
                .HasMaxLength(500);
        }
    }
}
