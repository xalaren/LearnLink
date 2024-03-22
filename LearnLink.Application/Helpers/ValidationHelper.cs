namespace LearnLink.Application.Helpers
{
    public static class ValidationHelper
    {
        public static bool ValidateToEmptyStrings(params string?[] args)
        {
            foreach(var arg in args)
            {
                if(string.IsNullOrWhiteSpace(arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateToStringLength(string value, int maxLength)
        {
            return value.Length <= maxLength;
        }
    }
}
