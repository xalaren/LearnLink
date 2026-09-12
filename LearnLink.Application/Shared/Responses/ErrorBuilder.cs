namespace LearnLink.Application.Shared.Responses;

/// <summary>
/// Builder used to accumulate a collection of <see cref="Error"/> instances.
/// Provides a fluent API to add errors by code and message.
/// </summary>
public class ErrorsBuilder
{
    private readonly List<Error> _errors = [];

    /// <summary>
    /// Begins building an error identified by the provided code.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <returns>An <see cref="ErrorBuilder{TNext}"/> to set details for the error.</returns>
    public ErrorBuilder<ErrorsBuilder> For(string code)
    {
        ArgumentNullException.ThrowIfNull(code, nameof(code));

        return new(this, Add, code);
    }

    /// <summary>
    /// Builds and returns the accumulated errors.
    /// </summary>
    public Error[] Build()
    {
        return _errors.ToArray();
    }

    private void Add(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        _errors.Add(error);
    }
}

/// <summary>
/// Helper used by <see cref="ErrorsBuilder"/> to provide a fluent API for
/// setting an error message and completing the addition.
/// </summary>
/// <typeparam name="TNext">The next builder type returned after finalizing the error.</typeparam>
public class ErrorBuilder<TNext> where TNext : class
{
    private readonly string _code;
    private string _message = string.Empty;
    private readonly Action<Error> _addAction;
    private readonly TNext _nextBuilder;

    public ErrorBuilder(TNext nextBuilder, Action<Error> addAction, string code)
    {
        ArgumentNullException.ThrowIfNull(nextBuilder);
        ArgumentNullException.ThrowIfNull(addAction);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        _nextBuilder = nextBuilder;
        _addAction = addAction;
        _code = code;
    }

    /// <summary>
    /// Sets the human-readable message for the error being built.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <returns>The current builder for chaining.</returns>
    public ErrorBuilder<TNext> WithMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        _message = message;

        return this;
    }

    /// <summary>
    /// Finalizes the error and returns the next builder in the chain.
    /// </summary>
    public TNext Then()
    {
        var error = new Error(_code, _message);
        _addAction(error);

        return _nextBuilder;
    }
}
