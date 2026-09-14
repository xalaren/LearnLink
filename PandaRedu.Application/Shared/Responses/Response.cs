using System.Text.Json;
using System.Text.Json.Serialization;
using PandaRedu.Application.Shared.Responses.Enums;

namespace PandaRedu.Application.Shared.Responses;

/// <summary>
/// Represents a generic response produced by application handlers. Includes
/// a response type, success flag, optional message and details.
/// </summary>
/// <param name="Type">The response type (status).</param>
/// <param name="IsSuccess">Indicates whether the operation succeeded.</param>
/// <param name="Message">Optional human-readable message.</param>
/// <param name="Details">Optional error details.</param>
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

/// <summary>
/// Generic variant of <see cref="Response"/> that carries typed content.
/// </summary>
/// <typeparam name="TContent">The type of the content returned in the response.</typeparam>
/// <param name="Type">The response type (status).</param>
/// <param name="IsSuccess">Indicates whether the operation succeeded.</param>
/// <param name="Content">Optional typed content returned by the operation.</param>
/// <param name="Message">Optional human-readable message.</param>
/// <param name="Details">Optional error details.</param>
public record Response<TContent>(ResponseTypes Type, bool IsSuccess, TContent? Content, string? Message, Error[]? Details)
        : Response(Type, IsSuccess, Message, Details)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }
}
