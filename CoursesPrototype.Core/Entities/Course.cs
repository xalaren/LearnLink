namespace CoursesPrototype.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public bool IsPrivate { get; set; }

        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
