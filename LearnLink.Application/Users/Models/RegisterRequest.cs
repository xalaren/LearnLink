namespace LearnLink.Application.Users.Models
{
    public record RegisterRequest(string Nickname, string Name, string Lastname, DateTime? PasswordExpiration);
}
