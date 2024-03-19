using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class CourseCompletionEntityTypeConfiguration : IEntityTypeConfiguration<CourseCompletion>
    {
        public void Configure(EntityTypeBuilder<CourseCompletion> builder)
        {
            builder.HasKey(courseCompletion => new { courseCompletion.UserId, courseCompletion.CourseId });
        }
    }
}
