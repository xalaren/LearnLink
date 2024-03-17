namespace LearnLink.Core.Entities
{
    public class UserCourseLocalRole
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LocalRoleId { get; set; }
        public LocalRole LocalRole { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
