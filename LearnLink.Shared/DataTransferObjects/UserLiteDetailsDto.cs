namespace LearnLink.Shared.DataTransferObjects
{
    public class UserLiteDetailsDto
    {
        public int Id { get; init; }
        public string Nickname { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string Lastname { get; init; } = null!;
        public string? AvatarUrl { get; init; } = null!;
    }
}
