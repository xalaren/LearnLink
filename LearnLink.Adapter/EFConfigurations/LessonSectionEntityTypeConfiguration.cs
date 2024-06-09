using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations;

public class LessonSectionEntityTypeConfiguration : IEntityTypeConfiguration<LessonSection>
{
    public void Configure(EntityTypeBuilder<LessonSection> builder)
    {
        builder
            .HasKey(lessonSection => new { lessonSection.LessonId, lessonSection.SectionId });
        
    }
}