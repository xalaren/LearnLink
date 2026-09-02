using System.Reflection.Emit;

namespace LearnLink.Domain.Entities.Abstractions
{
    public interface ITypedKey<TContainer, TKey> : IEmptyable<TContainer>, IEquatable<TContainer>
        where TContainer : struct
        where TKey : struct
    {
        TKey Value { get; }
        static abstract TContainer Empty();
        static abstract TContainer New();
        static abstract TContainer Parse(string value);

    }
}
