namespace LearnLink.Core.Interfaces
{
    public interface ICompletion
    {
        public bool Completed { get; set; }
        public int CompletionProgress { get; set; }
    }
}
