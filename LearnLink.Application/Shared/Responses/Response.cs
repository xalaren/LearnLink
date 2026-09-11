using System.Text.Json;
using System.Text.Json.Serialization;
using LearnLink.Application.Shared.Responses.Enums;

namespace LearnLink.Application.Shared.Responses;

public record Response(ResponseTypes Type, bool IsSuccess, string? Message, Error[]? Details)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }

    protected static readonly JsonSerializerOptions WriteOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true
    };
}

public record Response<TContent>(ResponseTypes Type, bool IsSuccess, TContent? Content, string? Message, Error[]? Details)
        : Response(Type, IsSuccess, Message, Details)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }
}
