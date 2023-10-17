namespace CoursesPrototype.Application.Security
{
    public static class HashGenerator
    {
        public static string Generate(string password, string salt) => SHA256Encryption.ComputeSha256Hash(password + salt);
    }
}
