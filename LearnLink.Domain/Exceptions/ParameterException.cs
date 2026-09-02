using System.Text;

namespace LearnLink.Domain.Exceptions
{
    public class ParameterException : DomainException
    {
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
}
