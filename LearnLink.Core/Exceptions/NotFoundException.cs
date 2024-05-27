using System.Diagnostics.CodeAnalysis;

namespace LearnLink.Core.Exceptions
{
    public class NotFoundException(string message) : CustomException(message)
    {
        public override int StatusCode => 404;

        [DoesNotReturn]
        public static void ThrowIfNull(object? obj, string message)
        {
            if (obj is null)
            {
                Throw(message);   
            }
        }
        
        [DoesNotReturn]
        private static void Throw (string message) => throw new NotFoundException(message);
    }
}