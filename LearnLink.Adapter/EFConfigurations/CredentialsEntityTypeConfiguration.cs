using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class CredentialsEntityTypeConfiguration : IEntityTypeConfiguration<Credentials>
    {
        public void Configure(EntityTypeBuilder<Credentials> builder)
        {

        }
    }
}
