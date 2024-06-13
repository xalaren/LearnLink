using System.Diagnostics.CodeAnalysis;

namespace LearnLink.Core.Exceptions
{
    public class NotFoundException(string message) : CustomException(message)
    {
        public override int StatusCode => 404;

        public static void ThrowIfNotFound([NotNull]object? obj, string message)
        {
            if (obj is null)
            {
                Throw(message);
            }
        }

        [DoesNotReturn]
        public static void Throw(string message) =>
                throw new NotFoundException(message);
    }
}