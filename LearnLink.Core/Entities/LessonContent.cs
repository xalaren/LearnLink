namespace LearnLink.Core.Entities
{
    public class LessonContent
    {
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;
    }
}
