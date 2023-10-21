using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesPrototype.Adapter.EFConfigurations
{
    internal class SubscriptionsEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            //builder.HasKey(subscription => new { subscription.UserId, subscription.CourseId });
        }
    }
}
