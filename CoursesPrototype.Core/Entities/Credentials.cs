namespace CoursesPrototype.Core.Entities
{
    public class Credentials
    {
        public int UserId { get; set; }

        public string Nickname { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public string Salt { get; init; } = null!;
    }
}
