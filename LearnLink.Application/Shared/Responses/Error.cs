using System.Text.Json;

namespace LearnLink.Application.Shared.Responses;

public record Error(string Code, string Message)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true
    };
}
