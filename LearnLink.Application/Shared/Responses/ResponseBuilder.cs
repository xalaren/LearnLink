using LearnLink.Application.Shared.Responses.Enums;

namespace LearnLink.Application.Shared.Responses;

public class ResponseBuilder
{
    protected ResponseTypes Type { get; set; }
    protected bool IsSuccess { get; set; }
    protected string? Message { get; set; }
    protected Error[]? Errors { get; set; }
    protected readonly ErrorsBuilder ErrorsBuilder = new();

    public ResponseBuilder Succeed()
    {
        Type = ResponseTypes.Succeeded;
        IsSuccess = true;

        return this;
    }

    public ResponseBuilder Forbid()
    {
        Type = ResponseTypes.Forbidden;
        IsSuccess = false;

        return this;
    }

    public ResponseBuilder Invalid()
    {
        Type = ResponseTypes.Invalid;
        IsSuccess = false;

        return this;
    }

    public ResponseBuilder NotFound()
    {
        Type = ResponseTypes.NotFound;
        IsSuccess = false;

        return this;
    }

    public ResponseBuilder Conflict()
    {
        Type = ResponseTypes.Conflict;
        IsSuccess = false;

        return this;
    }

    public ResponseBuilder Fail()
    {
        Type = ResponseTypes.Failed;
        IsSuccess = false;

        return this;
    }

    public ResponseBuilder<TContent> WithContent<TContent>(TContent content)
    {
        var builder = new ResponseBuilder<TContent>(content)
        {
            Type = Type,
            IsSuccess = IsSuccess,
            Message = Message,
            Errors = Errors
        };

        builder.CopyErrorBuilderFrom(ErrorsBuilder);

        return builder;
    }

    public ResponseBuilder WithDetails(Action<ErrorsBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        action(ErrorsBuilder);

        return this;
    }

    public ResponseBuilder WithDetails(Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        Errors = errors;

        return this;
    }

    public ResponseBuilder WithMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));

        Message = message;
        return this;
    }

    public Response Build()
    {
        return new Response
        (
            Type: Type,
            IsSuccess: IsSuccess,
            Message: Message,
            Details: GetDetails()
        );
    }

    protected Error[]? GetDetails()
    {
        if (Errors is not null && Errors.Length > 0)
            return Errors;

        var built = ErrorsBuilder.Build();
        return built is not null && built.Length > 0 ? built : null;
    }
}

public sealed class ResponseBuilder<TContent> : ResponseBuilder
{
    private TContent? _content;

    public ResponseBuilder()
    {

    }

    public ResponseBuilder(TContent? content)
    {
        _content = content;
    }

    public new ResponseBuilder<TContent> Succeed()
    {
        base.Succeed();
        return this;
    }

    public new ResponseBuilder<TContent> Invalid()
    {
        base.Invalid();
        return this;
    }

    public new ResponseBuilder<TContent> Forbid()
    {
        base.Forbid();
        return this;
    }

    public new ResponseBuilder<TContent> NotFound()
    {
        base.NotFound();
        return this;
    }

    public new ResponseBuilder<TContent> Conflict()
    {
        base.Conflict();
        return this;
    }

    public new ResponseBuilder<TContent> Fail()
    {
        base.Fail();
        return this;
    }

    public new ResponseBuilder<TContent> WithDetails(Action<ErrorsBuilder> action)
    {
        base.WithDetails(action);
        return this;
    }

    public new ResponseBuilder<TContent> WithDetails(Error[] errors)
    {
        base.WithDetails(errors);
        return this;
    }

    public new ResponseBuilder<TContent> WithMessage(string message)
    {
        base.WithMessage(message);
        return this;
    }

    public ResponseBuilder<TContent> WithContent(TContent content)
    {
        _content = content;
        return this;
    }

    internal void CopyErrorBuilderFrom(ErrorsBuilder source)
    {
        var alreadyBuilt = source.Build();
        if (alreadyBuilt is { Length: > 0 })
            WithDetails(alreadyBuilt);
    }

    public new Response<TContent> Build()
    {
        return new Response<TContent>
        (
            Type: Type,
            IsSuccess: IsSuccess,
            Content: _content,
            Message: Message,
            Details: GetDetails()
        );
    }
}
