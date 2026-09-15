using System.Text;

namespace RustyTail.Domain.Exceptions;

/// <summary>
/// Domain exception type of parameter invalidation
/// </summary>
public class ParameterException : DomainException
{
    /// <summary>
    /// Name of parameter
    /// </summary>
    public string ParameterName { get; }

    public ParameterException(string message, string parameterName) : base(message)
    {
        ParameterName = parameterName;
    }

    public override string ToString()
    {
        return Format
        (
            $"Parameter failed: {ParameterName}"
        );
    }

    /// <summary>
    /// Formatting exception fields
    /// </summary>
    /// <param name="fields">Exception fields</param>
    /// <returns>Formatted string representation of exception</returns>
    protected virtual string Format(params string[] fields)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("\n{");

        foreach (var field in fields)
        {
            builder.AppendLine(field + ", ");
        }

        builder.AppendLine("Exception: " + base.ToString());

        builder.AppendLine("}");

        return builder.ToString();
    }
}
