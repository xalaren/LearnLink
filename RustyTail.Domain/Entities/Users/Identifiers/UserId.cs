using RustyTail.Domain.Entities.Abstractions;
using RustyTail.Domain.Entities.Users.Models;

namespace RustyTail.Domain.Entities.Users.Identifiers;

/// <summary>
/// Strongly typed primary key of <see cref="User"/> entity
/// </summary>
/// <param name="Value">Guid representation</param>
public readonly record struct UserId(Guid Value) : ITypedKey<UserId, Guid>
{
    public static UserId Empty() => new(Guid.Empty);
    public static UserId New() => new(Guid.NewGuid());
    public static UserId Parse(string value) => new(Guid.Parse(value));
    public static bool IsEmpty(UserId userId) => userId.Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
