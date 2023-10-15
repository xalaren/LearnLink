namespace CoursesPrototype.Core.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
