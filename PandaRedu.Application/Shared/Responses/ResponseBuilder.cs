using PandaRedu.Application.Shared.Responses.Enums;

namespace PandaRedu.Application.Shared.Responses;

/// <summary>
/// Helper used to build <see cref="Response"/> instances using a fluent API.
/// Use this builder to set response type, message and details and then
/// create either a non-generic or generic response.
/// </summary>
public class ResponseBuilder
{
    protected ResponseTypes Type { get; set; }
    protected bool IsSuccess { get; set; }
    protected string? Message { get; set; }
    protected Error[]? Errors { get; set; }
    protected readonly ErrorsBuilder ErrorsBuilder = new();

    /// <summary>
    /// Marks response as Succeeded
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder Succeed()
    {
        Type = ResponseTypes.Succeeded;
        IsSuccess = true;

        return this;
    }

    /// <summary>
    /// Marks response as Forbidden
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder Forbid()
    {
        Type = ResponseTypes.Forbidden;
        IsSuccess = false;

        return this;
    }

    /// <summary>
    /// Marks response as Invalid
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder Invalid()
    {
        Type = ResponseTypes.Invalid;
        IsSuccess = false;

        return this;
    }

    /// <summary>
    /// Marks response as NotFound
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder NotFound()
    {
        Type = ResponseTypes.NotFound;
        IsSuccess = false;

        return this;
    }

    /// <summary>
    /// Marks response as Conflict
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder Conflict()
    {
        Type = ResponseTypes.Conflict;
        IsSuccess = false;

        return this;
    }

    /// <summary>
    /// Marks response as Failed
    /// </summary>
    /// <returns>Current response builder</returns>
    public ResponseBuilder Fail()
    {
        Type = ResponseTypes.Failed;
        IsSuccess = false;

        return this;
    }

    /// <summary>
    /// Adds content and returns new generic response builder according to content type
    /// </summary>
    /// <returns>Current response builder</returns>
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

    /// <summary>
    /// Adds errors to details
    /// </summary>
    /// <param name="action">Delegate of errors builder</param>
    /// <returns>Current response builder</returns>
    public ResponseBuilder WithDetails(Action<ErrorsBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        action(ErrorsBuilder);

        return this;
    }

    /// <summary>
    /// Adds errors to details
    /// </summary>
    /// <param name="action">Array of errors</param>
    /// <returns>Current response builder</returns>
    public ResponseBuilder WithDetails(params Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        Errors = errors;

        return this;
    }

    /// <summary>
    /// Adds human-readable message
    /// </summary>
    /// <param name="message">Message</param>
    /// <returns>Current response builde</returns>
    public ResponseBuilder WithMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));

        Message = message;
        return this;
    }

    /// <summary>
    /// Builds new response
    /// </summary>
    /// <returns>New <see cref="Response"/> example</returns>

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

/// <summary>
/// Generic variant of <see cref="ResponseBuilder"/> that carries typed
/// content. Use <see cref="WithContent(TContent)"/> to set content before
/// calling <see cref="Build"/>.
/// </summary>
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

    ///  <inheritdoc cref="ResponseBuilder.Succeed"/>
    public new ResponseBuilder<TContent> Succeed()
    {
        base.Succeed();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.Invalid"/>
    public new ResponseBuilder<TContent> Invalid()
    {
        base.Invalid();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.Forbid"/>
    public new ResponseBuilder<TContent> Forbid()
    {
        base.Forbid();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.NotFound"/>
    public new ResponseBuilder<TContent> NotFound()
    {
        base.NotFound();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.Conflict"/>
    public new ResponseBuilder<TContent> Conflict()
    {
        base.Conflict();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.Fail"/>
    public new ResponseBuilder<TContent> Fail()
    {
        base.Fail();
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.WithDetails"/>
    public new ResponseBuilder<TContent> WithDetails(Action<ErrorsBuilder> action)
    {
        base.WithDetails(action);
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.WithDetails"/>
    public new ResponseBuilder<TContent> WithDetails(params Error[] errors)
    {
        base.WithDetails(errors);
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.WithMessage"/>
    public new ResponseBuilder<TContent> WithMessage(string message)
    {
        base.WithMessage(message);
        return this;
    }

    ///  <inheritdoc cref="ResponseBuilder.WithContent"/>
    public ResponseBuilder<TContent> WithContent(TContent content)
    {
        _content = content;
        return this;
    }

    /// <summary>
    /// Copies errors from respective error builder
    /// </summary>
    /// <param name="source">Error builder</param>
    internal void CopyErrorBuilderFrom(ErrorsBuilder source)
    {
        var alreadyBuilt = source.Build();
        if (alreadyBuilt is { Length: > 0 })
            WithDetails(alreadyBuilt);
    }

    ///  <inheritdoc cref="ResponseBuilder.Build"/>
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
