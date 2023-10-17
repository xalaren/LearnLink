using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CoursesPrototype.SecurityProvider;

public class AuthenticationOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public TimeSpan LifeTime { get; set; }
    public string SecretKey { get; set; } = null!;

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}
