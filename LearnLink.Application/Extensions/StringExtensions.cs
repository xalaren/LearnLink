namespace LearnLink.Application.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
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
