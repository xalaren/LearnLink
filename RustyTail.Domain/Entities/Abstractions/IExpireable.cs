namespace RustyTail.Domain.Entities.Abstractions;

/// <summary>
/// Provides expiration tracking
/// </summary>
public interface IExpireable
{
    /// <summary>
    /// Date of expiration (UTC time)
    /// </summary>
    DateTime ExpiresAtUtc { get; }

    /// <summary>
    /// Is expiration date has arrived
    /// </summary>
    bool IsExpired { get; }
}
