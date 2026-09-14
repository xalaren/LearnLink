using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PandaRedu.SecurityProvider;

/// <summary>
/// Configuration options used for creating authentication tokens.
/// Contains issuer/audience information, secret key and token lifetime.
/// </summary>
public record AuthenticationOptions
{
    /// <summary>
    /// Token issuer identifier.
    /// </summary>
    public required string Issuer { get; init; }

    /// <summary>
    /// Token audience identifier.
    /// </summary>
    public required string Audience { get; init; }

    /// <summary>
    /// Lifetime of issued tokens.
    /// </summary>
    public required TimeSpan LifeTime { get; init; }

    /// <summary>
    /// Secret key used to sign tokens.
    /// </summary>
    public required string SecretKey { get; init; }

    /// <summary>
    /// Symmetric security key derived from <see cref="SecretKey"/> encoded as UTF-8 bytes.
    /// </summary>
    public SymmetricSecurityKey SecurityKey => new(Encoding.UTF8.GetBytes(SecretKey));

    /// <summary>
    /// Creates a <see cref="SecurityTokenDescriptor"/> configured with the
    /// provided signing credentials and optional claims. The descriptor can
    /// be used by token handlers to create signed tokens.
    /// </summary>
    /// <param name="credentials">Signing credentials used to sign the token.</param>
    /// <param name="claims">Optional claims to include in the token subject.</param>
    /// <returns>A configured <see cref="SecurityTokenDescriptor"/>.</returns>
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
