using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using RustyTail.Application.Security.Providers;
using RustyTail.Domain.Entities.Users.Primitives;

namespace RustyTail.SecurityProvider;

/// <summary>
/// Provides password hashing and verification using Argon2id.
/// The provider produces a <see cref="Password"/> primitive containing
/// base64-encoded salt and hash values.
/// </summary>
public class EncryptionProvider : IEncryptionProvider
{
    // Cryptographic parameters
    private const int SaltSize = 16; // 128 bits
    private const int HashSize = 32; // 256 bits
    private const int Iterations = 3;
    private const int MemorySizeKb = 65536; // 64 MiB
    private const int DegreeOfParallelism = 2;

    /// <summary>
    /// Encrypts (hashes) the provided plain-text password and returns a
    /// <see cref="Password"/> containing the salt and hash encoded as Base64.
    /// </summary>
    /// <param name="plainPassword">The plain-text password to hash.</param>
    /// <returns>A <see cref="Password"/> containing the generated salt and hash.</returns>
    public Password Encrypt(string plainPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = ComputeHash(plainPassword, salt);

        return new Password(Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>
    /// Verifies that the provided plain-text password matches the stored
    /// <see cref="Password"/> by recomputing the hash using the stored salt
    /// and performing a fixed-time comparison.
    /// </summary>
    /// <param name="plainPassword">The plain-text password to verify.</param>
    /// <param name="password">The stored password primitive containing salt and hash.</param>
    /// <returns>True if the password matches; otherwise, false.</returns>
    public bool Verify(string plainPassword, Password password)
    {
        var salt = Convert.FromBase64String(password.Salt);
        var expectedHash = Convert.FromBase64String(password.Hash);
        var actualHash = ComputeHash(plainPassword, salt);

        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }

    /// <summary>
    /// Computes an Argon2id hash for the provided password and salt using
    /// configured parameters.
    /// </summary>
    /// <param name="plainPassword">The plain-text password.</param>
    /// <param name="salt">The salt bytes to use for hashing.</param>
    /// <returns>The computed hash bytes.</returns>
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
