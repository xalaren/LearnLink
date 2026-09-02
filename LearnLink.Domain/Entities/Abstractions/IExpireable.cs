namespace LearnLink.Core.Entities.Abstractions;

public interface IExpireable
{
    DateTime? ExpiresAtUtc { get; }
    bool NeededRefresh { get; }
}