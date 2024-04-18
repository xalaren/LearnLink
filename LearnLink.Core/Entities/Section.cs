namespace LearnLink.Core.Entities
{
    public class Section : Content
    {
        private const int ORDER_DEFAULT_VALUE = 1;
        private int order = ORDER_DEFAULT_VALUE;

        public int Id { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public string? Title { get; set; }

        public int Order
        {
            get => order;
            set
            {
                if (value < ORDER_DEFAULT_VALUE)
                {
                    order = ORDER_DEFAULT_VALUE;
                    return;
                }

                order = value;
            }
        }
    }
}
