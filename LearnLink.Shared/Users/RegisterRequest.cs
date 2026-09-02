namespace LearnLink.Shared.Users
{
    public record RegisterRequest(string Nickname, string Name, string Lastname, DateTime? PasswordExpiration);
}
