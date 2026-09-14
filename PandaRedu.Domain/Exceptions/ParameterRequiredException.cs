using System.Diagnostics.CodeAnalysis;

namespace PandaRedu.Domain.Exceptions;

/// <summary>
/// Parameter exception type of required cases
/// </summary>
/// <param name="message">Exception message</param>
/// <param name="parameterName">Name of parameter</param>
public class ParameterRequiredException(string message, string parameterName)
    : ParameterException(message, parameterName)
{
    private const string MessageTemplate = "{0} is required";

    /// <summary>
    /// Throws a new <see cref="ParameterRequiredException"/>
    /// </summary>
    /// <param name="parameterName">Name of parameter</param>
    /// <exception cref="ParameterRequiredException"></exception>
    [DoesNotReturn]
    public static void Throw(string parameterName)
    {
        throw new ParameterRequiredException(string.Format(MessageTemplate, parameterName), parameterName);
    }

    /// <summary>
    /// Throws a new <see cref="ParameterRequiredException"/>
    /// </summary>
    /// <param name="parameterName">Name of parameter</param>
    /// <param name="message">Exception message</param>
    /// <exception cref="ParameterRequiredException"></exception>
    [DoesNotReturn]
    public static void Throw(string parameterName, string message)
    {
        throw new ParameterRequiredException(message, parameterName);
    }


    public override string ToString()
    {
        return Format
        (
            $"Parameter required: {ParameterName}"
        );
    }
}
