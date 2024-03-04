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

            //builder.HasOne(cm => cm.Course)
            //    .WithMany(c => c.CourseModules)
            //    .HasForeignKey(cm => cm.CourseId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(cm => cm.Module)
            //    .WithOne(m => m.CourseModule)
            //    .HasForeignKey<CourseModule>(cm => cm.ModuleId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
