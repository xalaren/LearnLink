namespace LearnLink.Shared.DataTransferObjects
{
    public record ReviewDto
    {
        public int Id { get; init; }
        public int Grade { get; init; }
        public int ExpertUserId { get; init; }
        public UserLiteDetailsDto? ExpertUserDetails { get; init; }
        public string? Comment { get; init; }
        public string ReviewDate { get; init; } = string.Empty;
    }
}
