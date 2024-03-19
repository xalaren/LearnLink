using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class ModuleComletionEntityTypeConfiguration : IEntityTypeConfiguration<ModuleCompletion>
    {
        public void Configure(EntityTypeBuilder<ModuleCompletion> builder)
        {
            builder.HasKey(moduleCompletion => new { moduleCompletion.UserId, moduleCompletion.ModuleId });
        }
    }
}
