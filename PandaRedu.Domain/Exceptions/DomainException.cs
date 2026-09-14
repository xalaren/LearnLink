namespace PandaRedu.Domain.Exceptions;

/// <summary>
/// Domain exceptions
/// </summary>
public class DomainException : Exception
{
    public DomainException() : base() { }
    public DomainException(string message) : base(message) {  }
}
