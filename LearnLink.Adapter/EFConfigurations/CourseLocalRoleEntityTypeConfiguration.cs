using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class CourseLocalRoleEntityTypeConfiguration : IEntityTypeConfiguration<CourseLocalRole>
    {
        public void Configure(EntityTypeBuilder<CourseLocalRole> builder)
        {
            builder.HasKey(courseLocalRole => new { courseLocalRole.CourseId, courseLocalRole.LocalRoleId });
        }
    }
}
