using LearnLink.Application.Shared.Responses.Enums;

namespace LearnLink.Application.Shared.Responses;

public record Response(ResponseTypes Type, bool IsSuccess, string? Message, Error[]? Details);

public record Response<TContent>(ResponseTypes Type, bool IsSuccess, TContent? Content, string? Message, Error[]? Details)
        : Response(Type, IsSuccess, Message, Details);
