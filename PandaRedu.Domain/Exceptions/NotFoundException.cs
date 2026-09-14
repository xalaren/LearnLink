using System.Diagnostics.CodeAnalysis;

namespace PandaRedu.Domain.Exceptions;

/// <summary>
/// Domain exception type of not found resource
/// </summary>
/// <param name="message">Exception message</param>
public class NotFoundException(string message) : DomainException(message)
{
    /// <summary>
    /// Throws a new exception
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <exception cref="NotFoundException"></exception>
    [DoesNotReturn]
    public static void Throw(string message) => throw new NotFoundException(message);
}