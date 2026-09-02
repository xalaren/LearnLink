using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Domain.Entities.Users.Identifiers
{
    public readonly record struct UserId(Guid Value) : ITypedKey<UserId, Guid>
    {
        public static UserId Empty() => new(Guid.Empty);
        public static UserId New() => new(Guid.NewGuid());
        public static UserId Parse(string value) => new(Guid.Parse(value));
        public static bool IsEmpty(UserId userId) => userId.Value == Guid.Empty;
        public override string ToString() => Value.ToString();
    }
}
