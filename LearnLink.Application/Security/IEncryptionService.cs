namespace LearnLink.Application.Security
{
    public interface IEncryptionService
    {
        string GetRandomString(int size);
        string GetHash(string password, string salt);
    }
}
