namespace LearnLink.Core.Entities
{
    public class LessonObjective
    {
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        public int ObjectiveId { get; set; }
        public Objective Objective { get; set; } = null!;
    }
}
