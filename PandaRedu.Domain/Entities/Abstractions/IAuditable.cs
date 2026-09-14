namespace PandaRedu.Domain.Entities.Abstractions;

/// <summary>
/// Providing creation and modification audit
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Date of creation (UTC time)
    /// </summary>
    DateTime CreatedOnUtc { get; }

    /// <summary>
    /// Date of modification (UTC time)
    /// </summary>
    DateTime ModifiedOnUtc { get; }

    /// <summary>
    /// Sets CreatedOnUtc date
    /// </summary>
    /// <param name="utcNow">Created date by UTC</param>
    void SetCreated(DateTime utcNow);

    /// <summary>
    /// Sets ModifiedOnUtc date
    /// </summary>
    /// <param name="utcNow">Modified date by UTC</param>
    void SetModified(DateTime utcNow);
}