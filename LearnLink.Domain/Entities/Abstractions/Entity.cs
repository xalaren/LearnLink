namespace LearnLink.Domain.Entities.Abstractions;

/// <summary>
/// Base entity abstraction
/// </summary>
/// <typeparam name="TContainer">Container of primary key</typeparam>
public abstract class Entity<TContainer> : IAuditable, IEquatable<Entity<TContainer>>
    where TContainer : struct, ITypedKey<TContainer, Guid>
{
    /// <summary>
    /// Primary key
    /// </summary>
    public abstract TContainer Id { get; protected init; }

    /// <summary>
    /// Date of entity creation (UTC time)
    /// </summary>
    public DateTime CreatedOnUtc { get; private set; }

    /// <summary>
    /// Date of entity modification (UTC time)
    /// </summary>
    public DateTime ModifiedOnUtc { get; private set; }

    /// <summary>
    /// Sets CreatedOnUtc date
    /// </summary>
    /// <param name="utcNow">Created date by UTC</param>
    public void SetCreated(DateTime utcNow)
    {
        CreatedOnUtc = utcNow;
    }

    /// <summary>
    /// Sets ModifiedOnUtc date
    /// </summary>
    /// <param name="utcNow">Modified date by UTC</param>
    public void SetModified(DateTime utcNow)
    {
        ModifiedOnUtc = utcNow;
    }

    public bool Equals(Entity<TContainer>? other)
    {
        return other is not null && GetType() == other.GetType() && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}