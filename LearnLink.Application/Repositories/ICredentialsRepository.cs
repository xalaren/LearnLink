using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Application.Repositories
{
    public interface ICredentialsRepository
    {
        void Add(Credentials credentials);
    }
}
