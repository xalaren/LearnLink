namespace LearnLink.Domain.Entities.Abstractions;

public interface IExpireable
{
    DateTime ExpiresAtUtc { get; }
    bool IsExpired { get; }
}
