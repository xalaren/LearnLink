namespace LearnLink.Domain.Guards;

public readonly struct AgainstClause<T>
{
    public T Value { get; }
    public string ParameterName { get; }

    internal AgainstClause(T value, string parameterName)
    {
        Value = value;
        ParameterName = parameterName;
    }
}
