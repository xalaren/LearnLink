namespace LearnLink.Shared.DataTransferObjects
{
    public class ClientModuleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Completed { get; set; } = false;
        public int? CompletionProgress { get; set; }
    }
}
