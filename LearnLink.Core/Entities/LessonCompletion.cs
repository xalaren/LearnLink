namespace LearnLink.Core.Entities
{
    public class LessonCompletion : Completion
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;
    }
}
