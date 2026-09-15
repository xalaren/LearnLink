namespace RustyTail.Domain.Entities.Abstractions;

/// <summary>
/// Provides IsEmpty check
/// </summary>
/// <typeparam name="T">Type of checking value</typeparam>
public interface IEmptyable<T>
{
    /// <summary>
    /// Is value empty
    /// </summary>
    /// <param name="value">Respective value</param>
    /// <returns><br>true if value is empty</br><br>false if value is not empty</br></returns>
    static abstract bool IsEmpty(T value);
}
