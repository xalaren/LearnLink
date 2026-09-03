namespace LearnLink.Shared.Model.Users
{
    public record RegisterRequest(string Nickname, string Name, string Lastname, DateTime? PasswordExpiration);
}
