namespace LearnLink.Core.Entities
{
    public class Credentials
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string HashedPassword { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}
