using System.Text.Json;
using System.Text.Json.Serialization;
using LearnLink.Application.Shared.Responses.Enums;

namespace LearnLink.Application.Shared.Responses;

public record Response(ResponseTypes Type, bool IsSuccess, string? Message, Error[]? Details)
{
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}

public record Response<TContent>(ResponseTypes Type, bool IsSuccess, TContent? Content, string? Message, Error[]? Details)
        : Response(Type, IsSuccess, Message, Details)
{
    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, options);
    }
}
