namespace PandaRedu.Domain.Entities.Abstractions;

/// <summary>
/// Provides custom primary key definition
/// </summary>
/// <typeparam name="TContainer">Type of implementation</typeparam>
/// <typeparam name="TKey">Type of key</typeparam>
public interface ITypedKey<TContainer, TKey> : IEmptyable<TContainer>, IEquatable<TContainer>
    where TContainer : struct, ITypedKey<TContainer, TKey>
    where TKey : struct
{
    /// <summary>
    /// Key value
    /// </summary>
    TKey Value { get; }

    /// <summary>
    /// Creates empty key
    /// </summary>
    /// <returns>Empty key</returns>
    static abstract TContainer Empty();

    /// <summary>
    /// Creates a new key
    /// </summary>
    /// <returns>New key</returns>
    static abstract TContainer New();

    /// <summary>
    /// Parse key from string
    /// </summary>
    /// <param name="value">String value of key</param>
    /// <returns>Parsed key</returns>
    static abstract TContainer Parse(string value);

}
