using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LearnLink.SecurityProvider;

public record AuthenticationOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required TimeSpan LifeTime { get; init; }
    public required string SecretKey { get; init; }
    public SymmetricSecurityKey SecurityKey => new(Encoding.UTF8.GetBytes(SecretKey));

    internal SecurityTokenDescriptor ToTokenDescriptor(SigningCredentials credentials, params Claim[] claims)
    {
        return new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(LifeTime),
            SigningCredentials = credentials,
            Issuer = Issuer,
            Audience = Audience,
        };
    }
}
