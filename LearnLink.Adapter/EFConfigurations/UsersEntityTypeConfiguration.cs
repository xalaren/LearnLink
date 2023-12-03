using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class UsersEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(user => user.Nickname)
                .IsUnique();

            builder.Property(user => user.Nickname)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(user => user.Lastname)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
