using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CoursesPrototype.Application.Security;
using Microsoft.IdentityModel.Tokens;

namespace CoursesPrototype.SecurityProvider
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions authOptions;

        public AuthenticationService(AuthenticationOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public string? Authenticate(string nickname, string inputPassword, string storedPassword)
        {
            if (!string.Equals(inputPassword, storedPassword, StringComparison.InvariantCulture))
            {
                return null;
            }

            return GetToken(nickname);
        }

        public string GetToken(string nickname)
        {
            var identity = GetIdentity(nickname);

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

        private ClaimsIdentity GetIdentity(string nickname)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, nickname)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
