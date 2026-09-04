using LearnLink.Domain.Entities.Users.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.Configurations;

internal sealed class AvatarsEntityTypeConfiguration : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        builder
            .HasKey(avatar => avatar.Id);

        builder
            .Property(avatar => avatar.Id)
            .HasConversion
            (
                avatarId => avatarId.Value,
                value => new AvatarId(value)
            )
            .ValueGeneratedNever();

        builder
            .HasOne(avatar => avatar.User)
            .WithOne(user => user.Avatar)
            .HasForeignKey<Avatar>(avatar => avatar.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        builder
            .Property(avatar => avatar.UserId)
            .HasConversion
            (
                userId => userId.Value,
                value => new UserId(value)
            );

        builder
            .Property(avatar => avatar.Extension)
            .IsRequired()
            .HasMaxLength(Avatar.ExtensionMaxLength);
    }
}
