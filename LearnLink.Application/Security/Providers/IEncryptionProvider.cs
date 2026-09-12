using LearnLink.Domain.Entities.Users.Primitives;

namespace LearnLink.Application.Security.Providers
{
    /// <summary>
    /// Provides password encryption and verification utilities used by the
    /// application when creating or validating user credentials.
    /// </summary>
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the provided plain-text password into a <see cref="Password"/> primitive.
        /// </summary>
        /// <param name="plainPassword">The plain-text password to encrypt.</param>
        /// <returns>An encrypted <see cref="Password"/> representation.</returns>
        Password Encrypt(string plainPassword);

        /// <summary>
        /// Verifies whether the provided plain-text password matches the stored encrypted password.
        /// </summary>
        /// <param name="plainPassword">The plain-text password to verify.</param>
        /// <param name="password">The stored encrypted password.</param>
        /// <returns>True if the passwords match; otherwise false.</returns>
        bool Verify(string plainPassword, Password password);
    }
}
