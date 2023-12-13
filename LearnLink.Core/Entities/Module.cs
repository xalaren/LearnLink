namespace LearnLink.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public CourseModule CourseModule { get; set; } = null!;
        public ICollection<ModuleLesson> ModuleLessons { get; set; }  = new List<ModuleLesson>();
    }
}
