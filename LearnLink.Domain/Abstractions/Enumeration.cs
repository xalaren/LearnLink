namespace LearnLink.Domain.Abstractions
{
    public abstract class Enumeration<TEnum, TIdentifier> : IEquatable<Enumeration<TEnum, TIdentifier>> 
        where TEnum : Enumeration<TEnum, TIdentifier>
        where TIdentifier : struct
    {
        protected Enumeration(TIdentifier value, string name)
        {
            Value = value;
            Name = name;
        }

        public TIdentifier Value { get; protected init; }
        public string Name { get; protected init; } = string.Empty;

        public bool Equals(Enumeration<TEnum, TIdentifier>? other)
        {
            return other is not null && GetType() == other.GetType() && Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum, TIdentifier> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
