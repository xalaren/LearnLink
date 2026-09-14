using System.Security.Claims;
using System.Security.Cryptography;
using PandaRedu.Application.Security.Providers;
using PandaRedu.Domain.Entities.Users.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace PandaRedu.SecurityProvider;

/// <summary>
/// Produces access and refresh tokens using configured <see cref="AuthenticationOptions"/>.
/// </summary>
/// <param name="authenticationOptions">Options used to configure token generation (issuer, audience, keys, lifetime).</param>
public class TokenProvider(AuthenticationOptions authenticationOptions) : ITokenProvider
{
    private readonly AuthenticationOptions _authenticationOptions = authenticationOptions;

    /// <summary>
    /// Generates a signed access token for the specified user containing standard claims.
    /// </summary>
    /// <param name="user">The user for whom to generate the access token.</param>
    /// <returns>The generated access token as a string (JWT).</returns>
    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var claims = GetClaims(user).ToArray();
        var credentials = new SigningCredentials(_authenticationOptions.SecurityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = _authenticationOptions.ToTokenDescriptor(credentials, claims);
        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    /// <summary>
    /// Generates a cryptographically strong refresh token encoded as Base64.
    /// </summary>
    /// <returns>A new refresh token string.</returns>
    public string GenerateRefreshToken()
    {
        const int bytesLength = 32;
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(bytesLength));
    }

    private static IEnumerable<Claim> GetClaims(User user)
    {
        // Standard JWT claims plus role and nickname
        yield return new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString());
        yield return new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
        yield return new(JwtRegisteredClaimNames.Nickname, user.Nickname);
        yield return new(ClaimTypes.Role, user.RoleId.ToString());
    }
}