namespace LearnLink.Application.Shared.Responses;

public class ErrorsBuilder
{
    private readonly List<Error> _errors = [];

    public ErrorBuilder<ErrorsBuilder> For(string code)
    {
        ArgumentNullException.ThrowIfNull(code, nameof(code));

        return new(this, Add, code);
    }

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

    public ErrorBuilder<TNext> WithMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        _message = message;

        return this;
    }

    public TNext Then()
    {
        var error = new Error(_code, _message);
        _addAction(error);

        return _nextBuilder;
    }
}