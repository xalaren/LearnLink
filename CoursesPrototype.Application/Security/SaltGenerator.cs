using System.Security.Cryptography;

namespace CoursesPrototype.Application.Security
{
    public static class SaltGenerator
    {
        public static string Generate(int size)
        {
            byte[] saltBytes = new byte[size];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
    }
}
