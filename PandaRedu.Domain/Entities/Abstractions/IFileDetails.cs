namespace PandaRedu.Domain.Entities.Abstractions;

/// <summary>
/// Provides file details
/// </summary>
public interface IFileDetails
{
    /// <summary>
    /// File name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// File extension without '.' (e.g. png, svg, txt, cs, ...)
    /// </summary>
    string Extension { get; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    long Size { get; }

    /// <summary>
    /// File content type (e.g. images/png)
    /// </summary>
    string? ContentType { get; }
}
