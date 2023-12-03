namespace LearnLink.Core.Entities
{
    public class ModuleLesson
    {
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
