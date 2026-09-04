using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Domain.Entities.Users.Identifiers
{
    public readonly record struct RefreshTokenId(Guid Value) : ITypedKey<RefreshTokenId, Guid>
    {
        public static RefreshTokenId Empty() => new(Guid.Empty);
        public static RefreshTokenId New() => new(Guid.NewGuid());
        public static RefreshTokenId Parse(string value) => new(Guid.Parse(value));
        public static bool IsEmpty(RefreshTokenId value) => value.Value == Guid.Empty;
        public override string ToString() => Value.ToString();
    }
}
