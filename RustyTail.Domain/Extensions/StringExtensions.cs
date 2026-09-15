namespace RustyTail.Domain.Extensions;

/// <summary>
/// Extensions of <see cref="string"/>
/// </summary>
public static class StringExtensions
{
    extension(string value)
    {
        /// <summary>
        /// Converts string into camelCase format
        /// </summary>
        /// <returns>String in camelCase format</returns>
        public string ToCamelCase()
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (!char.IsUpper(value[0]))
                return value;

            var i = 0;
            while (i < value.Length && char.IsUpper(value[i]))
                i++;

            if (i == value.Length)
                return value.ToLowerInvariant();

            if (i <= 1)
                return char.ToLowerInvariant(value[0]) + value[1..];

            return value[..(i - 1)].ToLowerInvariant() + value[(i - 1)..];
        }
    }
}
