using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    internal class CourseModulesEntityTypeConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            builder.HasKey(courseModule => new { courseModule.CourseId, courseModule.ModuleId });
        }
    }
}
