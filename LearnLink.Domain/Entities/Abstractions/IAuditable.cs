namespace LearnLink.Core.Entities.Abstractions;

public interface IAuditable
{
    DateTime CreatedOnUtc { get; }
    DateTime ModifiedOnUtc { get; }

    void SetCreated(DateTime utcNow);
    void SetModified(DateTime utcNow);
}