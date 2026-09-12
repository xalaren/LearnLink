using System.Diagnostics.CodeAnalysis;

namespace LearnLink.Domain.Exceptions;

/// <summary>
/// Parameter exception type of overflow cases
/// </summary>
public class ParameterOverflowException : ParameterException
{
    /// <summary>
    /// Actual length of parameter
    /// </summary>
    public int ActualLength { get; }

    /// <summary>
    /// Allowed length of parameter
    /// </summary>
    public int AllowedLength { get; }
    public ParameterOverflowException(string message, string parameterName, int actualLength, int allowedLength) : base(message, parameterName)
    {
        ActualLength = actualLength;
        AllowedLength = allowedLength;
    }

    /// <summary>
    /// Throws new ParameterOverflowException
    /// </summary>
    /// <param name="parameterName">Name of parameter</param>
    /// <param name="actualLength">Actual length of parameter</param>
    /// <param name="allowedLength">Allowed parameter length</param>
    /// <exception cref="ParameterOverflowException"></exception>
    [DoesNotReturn]
    public static void Throw(string parameterName, int actualLength, int allowedLength)
    {
        throw new ParameterOverflowException($"Parameter length cannot be longer than {allowedLength} characters", parameterName, actualLength, allowedLength);
    }

    public override string ToString()
    {
        return Format
        (
            $"Parameter length overflow: {ParameterName}",
            $"Actual: {ActualLength}",
            $"Allowed: {AllowedLength}"
        );
    }
}
