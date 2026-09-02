using System.Diagnostics.CodeAnalysis;
using LearnLink.Domain.Entities.Abstractions;
using LearnLink.Domain.Entities.Users.Primitives;

namespace LearnLink.Domain.Exceptions
{
    public class ParameterRequiredException(string message, string parameterName)
        : ParameterException(message, parameterName)
    {
        private const string MessageTemplate = "{0} is required";

        [DoesNotReturn]
        public static void Throw(string parameterName)
        {
            throw new ParameterRequiredException(string.Format(MessageTemplate, parameterName), parameterName);
        }

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
}
