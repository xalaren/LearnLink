namespace LearnLink.Application.Security
{
    public interface IAuthenticationService
    {
        string? Authenticate(string nickname, string inputPassword, string storedPassword, string roleName);
        string GetToken(string nickname, string roleName);
    }
}
