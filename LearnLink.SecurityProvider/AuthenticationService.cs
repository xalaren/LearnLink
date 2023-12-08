using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LearnLink.Application.Security;
using Microsoft.IdentityModel.Tokens;

namespace LearnLink.SecurityProvider
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions authOptions;

        public AuthenticationService(AuthenticationOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public string? Authenticate(string nickname, string inputPassword, string storedPassword, string roleName)
        {
            if (!string.Equals(inputPassword, storedPassword, StringComparison.InvariantCulture))
            {
                return null;
            }

            return GetToken(nickname, roleName);
        }

        public string GetToken(string nickname, string roleName)
        {
            var identity = GetIdentity(nickname, roleName);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(authOptions.LifeTime),
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string nickname, string roleName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, nickname),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
