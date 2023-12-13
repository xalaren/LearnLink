using System.ComponentModel.DataAnnotations;

namespace LearnLink.Core.Entities
{
    public class Course
    {
        private string title = string.Empty;

        public int Id { get; set; }
        public bool IsPublic { get; set; } = false;
        public string? Description { get; set; }

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

        public int SubscribersCount { get; set; }

        public ICollection<CourseModule> CourseModules = new List<CourseModule>();
    }
}
