namespace LearnLink.Core.Entities
{
    public class UserLesson
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public bool IsCompleted { get; set; }
    }
}
