namespace LearnLink.Core.Entities
{
    public class AnswerReview
    {
        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = null!;

        public int ReviewId { get; set; }
        public Review Review { get; set; } = null!;
    }
}
