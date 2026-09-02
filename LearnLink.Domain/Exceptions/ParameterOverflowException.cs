using System.Diagnostics.CodeAnalysis;

namespace LearnLink.Domain.Exceptions
{
    public class ParameterOverflowException : ParameterException
    {
        public int ActualLength { get; }
        public int AllowedLength { get; }
        public ParameterOverflowException(string message, string parameterName, int actualLength, int allowedLength) : base(message, parameterName)
        {
            ActualLength = actualLength;
            AllowedLength = allowedLength;
        }

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
}
