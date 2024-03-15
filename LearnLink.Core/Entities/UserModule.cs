namespace LearnLink.Core.Entities
{
    public class UserModule
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public bool IsCompleted => CompletionPercent == 100;
        public int CompletionPercent { get; set; }
    }
}
