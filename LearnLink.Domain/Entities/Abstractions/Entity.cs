using LearnLink.Domain.Entities.Abstractions;

namespace LearnLink.Core.Entities.Abstractions;

public abstract class Entity<TContainer> : IAuditable, IEquatable<Entity<TContainer>>
    where TContainer : struct, ITypedKey<TContainer, Guid>
{
    public abstract TContainer Id { get; protected init; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ModifiedOnUtc { get; private set; }

    public void SetCreated(DateTime utcNow)
    {
        CreatedOnUtc = utcNow;
    }

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
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(TContainer other)
    {
        throw new NotImplementedException();
    }
}