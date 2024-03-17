using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    internal class UserCourseLocalRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserCourseLocalRole>
    {
        public void Configure(EntityTypeBuilder<UserCourseLocalRole> builder)
        {
            builder.HasKey(userCourseRole => new { userCourseRole.UserId, userCourseRole.CourseId, userCourseRole.LocalRoleId });
        }
    }
}
