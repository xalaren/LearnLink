using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Section
    {
        private int order = 1;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int Order
        {
            get => order;
            set
            {
                if (value < 1)
                {
                    throw new ValidationException("Порядок раздела не может быть меньше 1");
                }

                order = value;
            }
        }

    }

}
