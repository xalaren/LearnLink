using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class ModulesEntityTypeConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.Property(module => module.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(module => module.Description)
                .HasMaxLength(500);

            builder.HasOne(m => m.CourseModule)
                .WithOne(cm => cm.Module)
                .HasForeignKey<CourseModule>(cm => cm.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
