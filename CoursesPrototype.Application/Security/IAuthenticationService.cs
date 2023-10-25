namespace CoursesPrototype.Application.Security
{
    public interface IAuthenticationService
    {
        string? Authenticate(string nickname, string inputPassword, string storedPassword);
        string GetToken(string nickname);
    }
}
