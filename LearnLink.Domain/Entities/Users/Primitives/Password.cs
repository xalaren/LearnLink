using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Guards;
using LearnLink.Domain.Guards.Clauses;

namespace LearnLink.Domain.Entities.Users.Primitives
{
    public readonly record struct Password : IEmptyable<Password>
    {
        public string Salt { get; }
        public string Hash { get; }

        public Password(string salt, string hash)
        {
            Guard.For(salt).AgainstWhiteSpace();
            Guard.For(hash).AgainstWhiteSpace();

            Salt = salt;
            Hash = hash;
        }

        public static bool IsEmpty(Password value) => string.IsNullOrWhiteSpace(value.Salt) && string.IsNullOrWhiteSpace(value.Hash);
    }
}
