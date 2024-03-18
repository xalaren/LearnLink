using System.ComponentModel.DataAnnotations;

namespace LearnLink.Core.Entities
{
    public class Course
    {
        private string title = string.Empty;
        private int progressPercentage = 0;

        public int Id { get; set; }
        public bool IsPublic { get; set; } = false;
        public bool IsUnavailable { get; set; } = false;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ValidationException("Заголовок не был заполнен");
                }

                title = value;
            }
        }

        public int SubscribersCount { get; set; } = 0;

        public int ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                if (value < 0 && value > 100)
                {
                    throw new ValidationException("Процент прогресса не входит в пределы допустимого (0 - 100)");
                }
            }
        }
    }
}
