namespace LearnLink.Core.Entities
{
    public class CourseModule
    {
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
