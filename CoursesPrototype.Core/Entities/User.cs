namespace CoursesPrototype.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Nickname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
