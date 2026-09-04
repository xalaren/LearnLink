using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using LearnLink.Application.Security;
using LearnLink.Domain.Entities.Users.Primitives;

namespace LearnLink.SecurityProvider
{
    public class EncryptionProvider : IEncryptionProvider
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 3;
        private const int MemorySizeKb = 65536; // 64 MiB
        private const int DegreeOfParallelism = 2;

        public Password Encrypt(string plainPassword)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = ComputeHash(plainPassword, salt);

            return new Password(Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string plainPassword, Password password)
        {
            var salt = Convert.FromBase64String(password.Salt);
            var expectedHash = Convert.FromBase64String(password.Hash);
            var actualHash = ComputeHash(plainPassword, salt);

            return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
        }

        private static byte[] ComputeHash(string plainPassword, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(plainPassword))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySizeKb,
                DegreeOfParallelism = DegreeOfParallelism
            };

            return argon2.GetBytes(HashSize);
        }
    }
}
