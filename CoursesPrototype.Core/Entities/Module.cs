namespace CoursesPrototype.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
    }
}
