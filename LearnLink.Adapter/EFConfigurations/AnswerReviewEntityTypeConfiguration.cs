using LearnLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnLink.Adapter.EFConfigurations
{
    public class AnswerReviewEntityTypeConfiguration : IEntityTypeConfiguration<AnswerReview>
    {
        public void Configure(EntityTypeBuilder<AnswerReview> builder)
        {
            builder.HasKey(answerReview => new { answerReview.AnswerId, answerReview.ReviewId });
        }
    }
}
