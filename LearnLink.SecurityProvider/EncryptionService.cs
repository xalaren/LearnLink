using System.Security.Cryptography;
using System.Text;
using LearnLink.Application.Security;

namespace LearnLink.SecurityProvider
{
    public class EncryptionService : IEncryptionService
    {
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GetHash(string password, string salt) => ComputeSha256Hash(password + salt);

        public string GetRandomString(int size)
        {
            byte[] stringBytes = new byte[size];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(stringBytes);
            }

            return Convert.ToBase64String(stringBytes);
        }

    }
}
