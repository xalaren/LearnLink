using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class LessonCompletionEntityTypeConfiguration : IEntityTypeConfiguration<LessonCompletion>
    {
        public void Configure(EntityTypeBuilder<LessonCompletion> builder)
        {
            builder.HasKey(lessonCompletion => new { lessonCompletion.UserId, lessonCompletion.ModuleId, lessonCompletion.LessonId });
        }
    }
}
