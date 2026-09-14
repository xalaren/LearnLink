using PandaRedu.Domain.Entities.Users.Identifiers;
using PandaRedu.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PandaRedu.Adapter.Configurations;

internal class RefreshToeknEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder
            .Property(rt => rt.Id)
            .HasConversion
            (
                id => id.Value,
                value => new RefreshTokenId(value)
            )
            .ValueGeneratedNever();

        builder
            .HasOne(rt => rt.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(rt => rt.UserId)
            .HasConversion
            (
                userId => userId.Value,
                value => new UserId(value)
            );

        builder
            .Property(rt => rt.Token)
            .HasMaxLength(RefreshToken.TokenMaxLength)
            .IsRequired();

        builder
            .HasIndex(rt => rt.Token)
            .IsUnique();
    }
}
