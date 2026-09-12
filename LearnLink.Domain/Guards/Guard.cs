using System.Runtime.CompilerServices;

namespace LearnLink.Domain.Guards;

/// <summary>
/// Guards for protection against specific cases
/// </summary>
public static class Guard
{
    /// <summary>
    /// Applying guard for parameter
    /// </summary>
    /// <typeparam name="T">Type of parameter</typeparam>
    /// <param name="parameter">Respective parameter</param>
    /// <param name="expression">Caller method expression</param>
    /// <returns>Against clauses</returns>
    public static AgainstClause<T> For<T>(T parameter, [CallerArgumentExpression(nameof(parameter))] string expression = "")
    {
        return new AgainstClause<T>(parameter, expression);
    }
}