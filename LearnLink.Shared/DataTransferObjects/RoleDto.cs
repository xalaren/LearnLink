namespace LearnLink.Shared.DataTransferObjects
{
    public record RoleDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Sign { get; init; } = string.Empty;
    }
}
