namespace LearnLink.Domain.Entities.Abstractions;

public interface IEmptyable<T>
{
    static abstract bool IsEmpty(T value);
}
