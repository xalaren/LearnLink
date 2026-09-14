using PandaRedu.Domain.Entities.Users.Models;

namespace PandaRedu.Application.Security.Providers
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Generates a signed access token (JWT or similar) for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the access token.</param>
        /// <returns>The generated access token as a string.</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generates a new refresh token value.
        /// </summary>
        /// <returns>The generated refresh token as a string.</returns>
        string GenerateRefreshToken();
    }
}
