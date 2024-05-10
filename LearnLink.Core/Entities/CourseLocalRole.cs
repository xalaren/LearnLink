namespace LearnLink.Core.Entities
{
    public class CourseLocalRole
    {
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int LocalRoleId { get; set; }
        public LocalRole LocalRole { get; set; } = null!;
    }
}
