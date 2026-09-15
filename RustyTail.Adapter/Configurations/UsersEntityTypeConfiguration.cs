using RustyTail.Domain.Entities.Users.Identifiers;
using RustyTail.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RustyTail.Adapter.Configurations;

internal sealed class UsersEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder
            .Property(user => user.Id)
            .HasConversion(id => id.Value, value => new UserId(value))
            .ValueGeneratedNever();

        builder
            .Property(user => user.Nickname)
            .HasMaxLength(User.NicknameMaxLength)
            .IsRequired();

        builder
            .HasIndex(user => user.Nickname)
            .IsUnique();

        builder
            .Property(user => user.Name)
            .HasMaxLength(User.NameMaxLength)
            .IsRequired();

        builder
            .Property(user => user.Lastname)
            .HasMaxLength(User.LastnameMaxLength)
            .IsRequired();

        builder
            .Property(user => user.IsSystem)
            .IsRequired();

        builder
            .Property(user => user.RoleId)
            .HasConversion(id => id.Value, value => new RoleId(value));

        builder
            .HasOne(user => user.Role)
            .WithMany(role => role.Users)
            .HasForeignKey(user => user.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder
            .Property(user => user.AvatarId)
            .HasConversion
            (
                id => id == null ? (Guid?)null : id.Value.Value,
                value => value == null ? null : new AvatarId(value.Value)
            );

        builder
            .HasOne(u => u.Avatar)
            .WithOne()
            .HasForeignKey<User>(u => u.AvatarId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}