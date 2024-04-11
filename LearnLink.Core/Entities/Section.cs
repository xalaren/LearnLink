using LearnLink.Core.Exceptions;

namespace LearnLink.Core.Entities
{
    public class Section
    {
        private const int ORDER_DEFAULT_VALUE = 1;
        private int order = ORDER_DEFAULT_VALUE;
        private string title = string.Empty;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public string Title 
        {
            get => title;
            set 
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    title = GenerateTitle(order);
                }
            }
        }

        public int Order
        {
            get => order;
            set
            {
                if (value < ORDER_DEFAULT_VALUE)
                {
                    order = ORDER_DEFAULT_VALUE;
                }

                order = value;
            }
        }

        private string GenerateTitle(int order)
        {
            return $"Раздел {order}";
        }
    }
}
