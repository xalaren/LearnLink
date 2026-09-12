namespace LearnLink.Domain.Guards;

/// <summary>
/// Against clauses
/// </summary>
/// <typeparam name="T">Type of parameter value</typeparam>
public readonly struct AgainstClause<T>
{
    /// <summary>
    /// Value of respective parameter
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Name of respective parameter
    /// </summary>
    public string ParameterName { get; }

    internal AgainstClause(T value, string parameterName)
    {
        Value = value;
        ParameterName = parameterName;
    }
}
