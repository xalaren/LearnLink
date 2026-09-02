using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Domain.Entities.Users.Identifiers;

public readonly record struct AvatarId(Guid Value) : ITypedKey<AvatarId, Guid>
{
    public static AvatarId Empty() => new AvatarId(Guid.Empty);
    public static AvatarId New() => new AvatarId(Guid.NewGuid());
    public static AvatarId Parse(string value) => new AvatarId(Guid.Parse(value));
    public static bool IsEmpty(AvatarId avatarId) => avatarId.Value == Guid.Empty;
    public override string ToString() => Value.ToString();

}
