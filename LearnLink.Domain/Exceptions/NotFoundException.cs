using System.Diagnostics.CodeAnalysis;
using LearnLink.Domain.Exceptions;

namespace LearnLink.Core.Exceptions
{
    public class NotFoundException(string message) : DomainException(message)
    {
        public static void ThrowIfNotFound([NotNull]object? obj, string message)
        {
            if (obj is null)
            {
                Throw(message);
            }
        }

        [DoesNotReturn]
        public static void Throw(string message) => throw new NotFoundException(message);
    }
}