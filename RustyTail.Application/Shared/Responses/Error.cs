using System.Text.Json;

namespace RustyTail.Application.Shared.Responses;

/// <summary>
/// Represents a simple error with a code and a human-readable message.
/// </summary>
/// <param name="Code">Machine-readable error code.</param>
/// <param name="Message">Human-readable error message.</param>
public record Error(string Code, string Message)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true
    };
}
