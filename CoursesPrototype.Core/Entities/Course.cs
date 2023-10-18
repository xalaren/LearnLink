namespace CoursesPrototype.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsPublic { get; set; }
    }
}
