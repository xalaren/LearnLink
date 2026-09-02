using LearnLink.Domain.Entities.Users.Primitives;

namespace LearnLink.Application.Security
{
    public interface IEncryptionService
    {
        Password Encrypt(string plainPassword);
        bool Verify(string plainPassword, Password password);
    }
}
