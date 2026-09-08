using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Security.Providers
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
