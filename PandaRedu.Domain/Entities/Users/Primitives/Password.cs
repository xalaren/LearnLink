using PandaRedu.Domain.Entities.Abstractions;
using PandaRedu.Domain.Guards;
using PandaRedu.Domain.Guards.Clauses;

namespace PandaRedu.Domain.Entities.Users.Primitives;


/// <summary>
/// Password value object
/// </summary>
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
