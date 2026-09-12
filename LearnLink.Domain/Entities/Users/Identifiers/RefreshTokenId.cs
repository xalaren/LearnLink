using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Models;

namespace LearnLink.Domain.Entities.Users.Identifiers;

/// <summary>
/// Strongly typed primary key of <see cref="RefreshToken"/> entity
/// </summary>
/// <param name="Value">Guid representation</param>
public readonly record struct RefreshTokenId(Guid Value) : ITypedKey<RefreshTokenId, Guid>
{
    public static RefreshTokenId Empty() => new(Guid.Empty);
    public static RefreshTokenId New() => new(Guid.NewGuid());
    public static RefreshTokenId Parse(string value) => new(Guid.Parse(value));
    public static bool IsEmpty(RefreshTokenId value) => value.Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
