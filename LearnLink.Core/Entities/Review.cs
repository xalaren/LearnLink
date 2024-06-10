using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Review
    {
        private int grade = 0;

        public int Id { get; set; }

        public int Grade
        {
            get => grade;
            set
            {
                if (value < 0)
                {
                    throw new ValidationException("Оценка была вне диапазона");
                }
                if (value > 0)
                {
                    grade = value;
                }

            }
        }

        public string? Comment { get; set; }

        public int ExpertUserId { get; set; }
        public User ExpertUser { get; set; } = null!;
    }
}
