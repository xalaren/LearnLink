using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class LessonObjectiveEntityTypeConfiguration : IEntityTypeConfiguration<LessonObjective>
    {
        public void Configure(EntityTypeBuilder<LessonObjective> builder)
        {
            builder.HasKey(lessonObjective => new { lessonObjective.LessonId, lessonObjective.ObjectiveId });
        }
    }
}
