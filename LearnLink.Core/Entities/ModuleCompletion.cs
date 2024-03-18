using LearnLink.Core.Interfaces;

namespace LearnLink.Core.Entities
{
    public class ModuleCompletion : ICompletion
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public bool Completed { get; set; }
    }
}
