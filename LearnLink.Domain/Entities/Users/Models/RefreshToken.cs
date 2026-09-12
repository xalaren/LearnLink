using LearnLink.Domain.Constants;
using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Identifiers;
using LearnLink.Domain.Guards;
using LearnLink.Domain.Guards.Clauses;

namespace LearnLink.Domain.Entities.Users.Models;

public class RefreshToken : Entity<RefreshTokenId>, IExpireable
{
    private const int ExpirationDays = 7;
    public const int TokenMaxLength = TextLengthConstants.Medium;
    public sealed override RefreshTokenId Id { get; protected init; }
    public string Token { get; private set; } = null!;

    public UserId UserId { get; private set; }
    public User User { get; private set; } = null!;
    public DateTime ExpiresAtUtc { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;

    protected RefreshToken() { }
    private RefreshToken(RefreshTokenId id, UserId userId, string token)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAtUtc = NewExpireDate();
    }

    public static RefreshToken Create(UserId userId, string token)
    {
        Guard.For(userId).AgainstEmpty();
        Guard.For(token).AgainstWhiteSpace();

        return new(RefreshTokenId.New(), userId, token);
    }

    public void Refresh(string token)
    {
        Guard.For(token).AgainstWhiteSpace();

        Token = token;
        ExpiresAtUtc = NewExpireDate();
    }

    private static DateTime NewExpireDate() => DateTime.UtcNow.AddDays(ExpirationDays);
}
