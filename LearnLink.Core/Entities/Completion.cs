using System.ComponentModel.DataAnnotations;

namespace LearnLink.Core.Entities
{
    public class Completion
    {
        private int completionProgress = 0;
        private bool completed = false;

        public bool Completed
        {
            get => completed;
            set
            {
                completed = value;
                if (completed)
                {
                    completionProgress = 100;
                }
            }
        }

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
