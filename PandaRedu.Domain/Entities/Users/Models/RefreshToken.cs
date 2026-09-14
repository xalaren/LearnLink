using PandaRedu.Domain.Constants;
using PandaRedu.Domain.Entities.Abstractions;
using PandaRedu.Domain.Entities.Users.Identifiers;
using PandaRedu.Domain.Guards;
using PandaRedu.Domain.Guards.Clauses;

namespace PandaRedu.Domain.Entities.Users.Models;

/// <summary>
/// <see cref="RefreshToken"/> entity representation that stores a long-lived token
/// used to obtain new access tokens for a <see cref="User"/>.
/// </summary>
public class RefreshToken : Entity<RefreshTokenId>, IExpireable
{
    /// <summary>
    /// Number of days after which the refresh token expires.
    /// </summary>
    private const int ExpirationDays = 7;

    /// <summary>
    /// Maximum allowed length for the token string.
    /// </summary>
    public const int TokenMaxLength = TextLengthConstants.Medium;

    /// <summary>
    /// Identifier of the <see cref="RefreshToken"/> entity.
    /// </summary>
    public sealed override RefreshTokenId Id { get; protected init; }

    /// <summary>
    /// The refresh token value.
    /// </summary>
    public string Token { get; private set; } = null!;

    /// <summary>
    /// Foreign key referencing the <see cref="Models.User"/> owning this refresh token.
    /// </summary>
    public UserId UserId { get; private set; }

    /// <summary>
    /// Navigation property to the owner <see cref="Models.User"/>.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// UTC date/time when the token expires.
    /// </summary>
    public DateTime ExpiresAtUtc { get; private set; }

    /// <summary>
    /// True when the refresh token is expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;

    protected RefreshToken() { }
    private RefreshToken(RefreshTokenId id, UserId userId, string token)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAtUtc = NewExpireDate();
    }

    /// <summary>
    /// Creates new <see cref="RefreshToken"/> entity.
    /// </summary>
    /// <param name="userId">Foreign key for the owning <see cref="Models.User"/></param>
    /// <param name="token">Refresh token string</param>
    /// <returns>New <see cref="RefreshToken"/> entity</returns>
    public static RefreshToken Create(UserId userId, string token)
    {
        Guard.For(userId).AgainstEmpty();
        Guard.For(token).AgainstWhiteSpace();

        return new(RefreshTokenId.New(), userId, token);
    }

    /// <summary>
    /// Updates the token value and extends the expiration date.
    /// </summary>
    /// <param name="token">New refresh token string</param>
    public void Refresh(string token)
    {
        Guard.For(token).AgainstWhiteSpace();

        Token = token;
        ExpiresAtUtc = NewExpireDate();
    }

    /// <summary>
    /// Calculates a new expiration date based on the current UTC time and <see cref="ExpirationDays"/>.
    /// </summary>
    private static DateTime NewExpireDate() => DateTime.UtcNow.AddDays(ExpirationDays);
}
