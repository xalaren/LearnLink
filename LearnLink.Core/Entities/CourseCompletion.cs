using LearnLink.Core.Exceptions;
using LearnLink.Core.Interfaces;

namespace LearnLink.Core.Entities
{
    public class CourseCompletion : ICompletion
    {
        private int completionProgress = 0;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public bool Completed { get; set; }
        public int CompletionProgress
        {
            get => completionProgress;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ValidationException("Процент прохождения был вне допустимого диапазона");
                }

                if (value == 100)
                {
                    Completed = true;
                }
                else
                {
                    Completed = false;
                }
            }
        }
    }
}
