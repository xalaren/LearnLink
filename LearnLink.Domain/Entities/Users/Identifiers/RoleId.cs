using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Domain.Entities.Users.Identifiers;

public record struct RoleId(Guid Value) : ITypedKey<RoleId, Guid>
{
    public static RoleId Empty() => new(Guid.Empty);
    public static RoleId New() => new(Guid.NewGuid());
    public static RoleId Parse(string value) => new(Guid.Parse(value));
    public static bool IsEmpty(RoleId roleId) => roleId.Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
