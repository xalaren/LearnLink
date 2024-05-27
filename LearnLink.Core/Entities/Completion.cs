using System.ComponentModel.DataAnnotations;

namespace LearnLink.Core.Entities
{
    public class Completion
    {
        private int completionProgress = 0;
        private bool completed = false;

        public const int MAX_COMPLETION_VALUE = 100;

        public bool Completed
        {
            get => completed;
            set
            {
                completed = value;
                if (completed)
                {
                    completionProgress = MAX_COMPLETION_VALUE;
                }
            }
        }

        public int CompletionProgress
        {
            get => completionProgress;
            set
            {
                if (value < 0 || value > MAX_COMPLETION_VALUE)
                {
                    throw new ValidationException("Процент прохождения был вне допустимого диапазона");
                }

                completionProgress = value;
                
                Completed = value == MAX_COMPLETION_VALUE;
            }
        }
    }
}
