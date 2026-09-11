using System.Text.Json;

namespace LearnLink.Application.Shared.Responses;

public record Error(string Code, string Message)
{
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}
