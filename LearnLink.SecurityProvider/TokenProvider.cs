using System.Security.Claims;
using System.Security.Cryptography;
using LearnLink.Application.Security;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace LearnLink.SecurityProvider;

public class TokenProvider(AuthenticationOptions authenticationOptions) : ITokenProvider
{
    private readonly AuthenticationOptions _authenticationOptions = authenticationOptions;

    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var claims = GetClaims(user).ToArray();
        var credentials = new SigningCredentials(_authenticationOptions.SecurityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = _authenticationOptions.ToTokenDescriptor(credentials, claims);
        var handler = new JsonWebTokenHandler();
            
        return handler.CreateToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        const int bytesLength = 32;
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(bytesLength));
    }
        
    private static IEnumerable<Claim> GetClaims(User user)
    {
        yield return new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString());
        yield return new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
        yield return new(JwtRegisteredClaimNames.Nickname, user.Nickname);
        yield return new(ClaimTypes.Role, user.RoleId.ToString());
    }
}