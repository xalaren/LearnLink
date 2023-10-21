using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesPrototype.Adapter.EFConfigurations
{
    public class UserCreatedCoursesEntityTypeConfiguration : IEntityTypeConfiguration<UserCreatedCourse>
    {
        public void Configure(EntityTypeBuilder<UserCreatedCourse> builder)
        {
            builder.HasKey(u => new {u.UserId, u.CourseId});
        }
    }
}
