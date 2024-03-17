using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class LocalRoleEntityTypeConfiguration : IEntityTypeConfiguration<LocalRole>
    {
        public void Configure(EntityTypeBuilder<LocalRole> builder)
        {
            builder.Property(role => role.Sign)
                .HasMaxLength(50);

            builder.HasIndex(role => role.Sign)
                .IsUnique();
        }
    }
}
