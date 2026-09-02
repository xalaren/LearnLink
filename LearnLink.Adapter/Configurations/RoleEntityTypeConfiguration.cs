using LearnLink.Domain.Entities.Users.Identifiers;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.Configurations
{
    internal class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(role => role.Id);

            builder
                .Property(role => role.Id)
                .HasConversion
                (
                    roleId => roleId.Value,
                    value => new RoleId(value)
                )
                .ValueGeneratedNever();

            builder
                .HasMany(role => role.Users)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(role => role.Name)
                .HasMaxLength(Role.NameMaxLength)
                .IsRequired();

            builder
                .Property(role => role.IsAdmin)
                .IsRequired();

            builder
                .Property(role => role.IsSystem)
                .IsRequired();
        }
    }
}
